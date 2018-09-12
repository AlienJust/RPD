using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.PsnData.Contracts;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Big;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased
{
    internal sealed class PsnBinHandlerMappedLite : IPsnDataHandlerWithResources, IPsnDataHandlerIndexed,
        IPsnDataHandlerRetrieveCmdPartByPos
    {
        private readonly Func<Stream> _dataStreamGetter;
        private readonly IPsnPageExtractor _extractor;
        private readonly IPsnCommandPartSearcher _cmdPartSearcher;

        private IIdentifier _lastPsnProtocolId;

        private bool _areResourcesFree;
        private byte[] _goodPagesOnlyGoodBytes;
        private List<IPsnPageIndexRecord> _pagesIndex;
        private List<IPsnPageIndexRecord> _timeSortedGoodPages;
        private BigList<IPsnMapRecord> _commandsMap;


        public PsnBinHandlerMappedLite(Func<Stream> dataStreamGetter)
        {
            _dataStreamGetter = dataStreamGetter;
            _extractor = PsnPageExtractorFactory.Extractor;
            _cmdPartSearcher = new PsnCommandPartSearcherAllDefinedValuesAndCrc();
            _areResourcesFree = true;
        }


        private void RunThroughDataAndExectueCustomActionOnCommandPartsFound(
            IPsnProtocolCommandPartConfiguration[] commandParts, byte[] dataBytes,
            Func<IPsnCommandPartsPosition, int> maybeCmdPartFoundAction)
        {
            int zeroBasedCurrentByte = 0;
            try
            {
                var maxCmdLenInProtocol = commandParts.Max(cp => cp.Length); // TODO: Replace LINQ for perfomance

                while (true)
                {
                    var remainingLength = dataBytes.Length - zeroBasedCurrentByte - 1; // WHY? - To gain perfomance
                    IPsnProtocolCommandPartConfiguration[] commandPartsThatCouldBeInRemainingBytes;
                    if (remainingLength < maxCmdLenInProtocol)
                    {
                        commandPartsThatCouldBeInRemainingBytes =
                            commandParts.Where(cp => cp.Length <= remainingLength)
                                .ToArray(); // TODO: Replace LINQ for perfomance
                        if (commandPartsThatCouldBeInRemainingBytes.Length == 0)
                            break; // выход из цикла, если нет комманд, укалыдвающихся в оставшуюся длину потока данных
                    }
                    else
                    {
                        commandPartsThatCouldBeInRemainingBytes = commandParts;
                    }

                    int iterationBytesCount = 1;
                    for (int i = 0; i < commandPartsThatCouldBeInRemainingBytes.Length; ++i)
                    {
                        var commandPartThatCouldBeInRemainingBytes = commandPartsThatCouldBeInRemainingBytes[i];

                        var isHereCmdPartResult = _cmdPartSearcher.IsHereCmdPart(dataBytes, zeroBasedCurrentByte,
                            commandPartThatCouldBeInRemainingBytes);
                        if (isHereCmdPartResult == PsnCommandPartConfirmation.EverythyngIsOk)
                        {
                            iterationBytesCount = maybeCmdPartFoundAction.Invoke(new PsnCommandPartsAndPosition(
                                zeroBasedCurrentByte,
                                commandPartThatCouldBeInRemainingBytes));
                            break;
                        }
                    }

                    zeroBasedCurrentByte += iterationBytesCount;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при сканировании лога на наличие команд", ex);
            }

            Console.WriteLine("RunThroughDataAndExectueCustomActionOnCommandPartsFound complete");
        }


        IPsnMapSubrecord FuncGetPartRequest(IPsnMapRecord rec)
        {
            return rec.RequestRecordInfo;
        }


        IPsnMapSubrecord FuncGetPartReply(IPsnMapRecord rec)
        {
            return rec.ReplyRecordInfo;
        }


        public void FreeResources()
        {
            _commandsMap = null;
            _lastPsnProtocolId = null;
            _goodPagesOnlyGoodBytes = null;
            _timeSortedGoodPages = null;
            _pagesIndex = null;

            _areResourcesFree = true;
        }


        public void FillResources(
            IPsnProtocolConfiguration protocol,
            DateTime beginTime,
            IEnumerable<IPsnProtocolCommandPartConfigurationCycle> cycleCmdPartInfo,
            IEnumerable<IPsnProtocolCommandPartConfiguration> commandPartsToMap)
        {
            var goodCommandParts = protocol.CommandParts.Where(cp =>
                cp.Address != null && cp.CommandCode != null && cp.CrcHigh != null && cp.CrcLow != null).ToArray();

            byte[] dataBytes;
            using (var dataStream = _dataStreamGetter.Invoke())
            {
                dataBytes = new byte[dataStream.Length];
                dataStream.Read(dataBytes, 0, (int) dataStream.Length);
            }

            if (_pagesIndex == null)
            {
                var fr = BuildPagesIndex(dataBytes);
                _pagesIndex = fr.FullIndex;
                _timeSortedGoodPages = fr.TimeSortedGoodPages;
                _goodPagesOnlyGoodBytes = GetOnlyGoodData(fr.TimeSortedGoodPages, dataBytes);

                _commandsMap = BuildCommandsMap(goodCommandParts, cycleCmdPartInfo, _goodPagesOnlyGoodBytes, beginTime,
                    _timeSortedGoodPages);
            }

            _lastPsnProtocolId = protocol.Information.Id;
            _areResourcesFree = false;
        }


        private FirstRunResult BuildPagesIndex(byte[] dataBytes)
        {
            var result = new List<IPsnPageIndexRecord>();

            var resultGoodBeforeTimebreak = new List<IPsnPageIndexRecord>();
            var resultGoodAfterTimebreak = new List<IPsnPageIndexRecord>();
            var resultGoodCurrent = resultGoodBeforeTimebreak;

            int msToAdd;
            try
            {
                msToAdd = int.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Ms.txt")));
            }
            catch
            {
                msToAdd = 0;
            }

            IPsnPageIndexRecord prevGoodPageWithTime = null;

            var pagesCount = dataBytes.Length / (uint) _extractor.PsnPageSize;

            for (int i = 0; i < pagesCount; ++i)
            {
                int absPosInData = i * _extractor.PsnPageSize;
                var header = _extractor.GetHeaderFromRealDevice(dataBytes, absPosInData);

                //var pageIndex = new PsnPageIndexRecord(absolutePositionInStream, header.PageInfo, header.CreatedAt, header.PageNumber, i);
                var page = new PsnPageIndexRecord(absPosInData, header.PageInfo,
                    header.CreatedAt + TimeSpan.FromMilliseconds(msToAdd * i), header.PageNumber, i);
                result.Add(page);

                // Выполнение условия переформирования лога по месту перехода с отрицательной дельтой времени:
                if (page.PageInfo == PsnPageInfo.NormalPage)
                {
                    if (page.PageTime.HasValue)
                    {
                        if (page.PageTime.Value < prevGoodPageWithTime?.PageTime.Value)
                        {
                            resultGoodCurrent = resultGoodAfterTimebreak;
                        }

                        prevGoodPageWithTime =
                            page; // Предыдущая страница ВСЕГДА валидная и имеет валидную временную метку
                    }

                    resultGoodCurrent.Add(page);
                }
            }


            resultGoodAfterTimebreak.AddRange(resultGoodBeforeTimebreak);
            resultGoodBeforeTimebreak.Clear();

            return new FirstRunResult {FullIndex = result, TimeSortedGoodPages = resultGoodAfterTimebreak};
        }


        /// <summary>
        /// Возвращает массив байт, в которых только хорошие данные
        /// </summary>
        /// <param name="goodPages">Список хороших страниц</param>
        /// <param name="dataBytes">Общий массив данных</param>
        /// <returns></returns>
        private byte[] GetOnlyGoodData(List<IPsnPageIndexRecord> goodPages, byte[] dataBytes)
        {
            int pageDataSize = _extractor.PsnPageSize - _extractor.PsnPageHeaderLength;
            byte[] result = new byte[goodPages.Count * pageDataSize];
            for (int i = 0; i < goodPages.Count; ++i)
            {
                for (int j = 0; j < pageDataSize; ++j)
                {
                    result[i * pageDataSize + j] =
                        dataBytes[goodPages[i].AbsolutePositionInStream + _extractor.PsnPageHeaderLength + j];
                }
            }

            return result;
        }


        private BigList<IPsnMapRecord> BuildCommandsMap(IPsnProtocolCommandPartConfiguration[] cmdParts,
            IEnumerable<IPsnProtocolCommandPartConfigurationCycle> cycleCmdPartInfos, byte[] dataBytes,
            DateTime psnLogBeginTime, List<IPsnPageIndexRecord> pagesIndex)
        {
            // Структура записи будет:
            // Запись:
            //     Запрос: 
            //         - позиция
            //         - кусок команды
            //     Ответ:  
            //         - позиция
            //         - кусок команды
            var result = new BigList<IPsnMapRecord>();
            var time = psnLogBeginTime;
            var cycleCmdPartInfoArray = cycleCmdPartInfos as IPsnProtocolCommandPartConfigurationCycle[] ??
                                        cycleCmdPartInfos.ToArray();
            IPsnMapSubrecord prevRequestSubrec = null; // Запрос из предыдущей итерации (изначально его нет)

            RunThroughDataAndExectueCustomActionOnCommandPartsFound(
                cmdParts,
                dataBytes,
                partsPosition =>
                {
                    try
                    {
                        IPsnCommandPartsPosition currPtLocInfo =
                            new PsnCommandPartsAndPosition(partsPosition.DataPosition,
                                partsPosition.CrcOkPartLocationInfo);
                        var pageIndex = currPtLocInfo.DataPosition /
                                        (_extractor.PsnPageSize - _extractor.PsnPageHeaderLength);

                        var pageWhereCmdPartLocated = pagesIndex[pageIndex];
                        // Подгонка времени до страничного, если время теущей страницы больше времени
                        if (pageWhereCmdPartLocated.PageTime.HasValue)
                        {
                            if (pageWhereCmdPartLocated.PageTime.Value > time)
                            {
                                time = pageWhereCmdPartLocated.PageTime.Value;
                            }
                        }

                        // -1. Если среди текущих найденных кусков команд есть циклическай кусок с правильной контрольной суммой, то время инкрементируется:
                        for (int j = 0; j < cycleCmdPartInfoArray.Length; ++j)
                        {
                            if (currPtLocInfo.CrcOkPartLocationInfo.Id.IdentyString ==
                                cycleCmdPartInfoArray[j].Id.IdentyString)
                            {
                                time = time.Add(cycleCmdPartInfoArray[j].CyclePeriod);
                                break;
                            }
                        }


                        // 0. К этой точке есть готовое время для текущей части команды - переменная time

                        var currentTruelyFoundCommandPart = currPtLocInfo.CrcOkPartLocationInfo;

                        bool isCurrentTruelyFoundCommandPartRepliedOnPrevRequest =
                            false; // является ли точно найденная команда ответом на предыдущий запрос
                        // Если предыдущий запрос есть, то вероятно надо искать ответ на него:
                        if (prevRequestSubrec != null)
                        {
                            if (currentTruelyFoundCommandPart.PartType == PsnProtocolCommandPartType.Reply ||
                                currentTruelyFoundCommandPart.PartType ==
                                PsnProtocolCommandPartType.ReplyWithRequiredRequest)
                            {
                                if (prevRequestSubrec.CommandPart.CommandId.IdentyString ==
                                    currentTruelyFoundCommandPart.CommandId.IdentyString)
                                {
                                    isCurrentTruelyFoundCommandPartRepliedOnPrevRequest = true;
                                }
                            }
                        }

                        if (currentTruelyFoundCommandPart.PartType == PsnProtocolCommandPartType.Request)
                        {
                            // Если до этого уже был запрос, то он сохраняется в карту как безответная запись:
                            if (prevRequestSubrec != null)
                            {
                                result.Add(new PsnMapRecord(
                                    prevRequestSubrec.CommandPart.CommandId,
                                    prevRequestSubrec,
                                    null));
                            }

                            // Текущий запрос сохраняется, для дальнейшего поиска ответа на него:
                            prevRequestSubrec = new PsnMapSubrecord(
                                new PsnCommandPartLocation(
                                    currPtLocInfo.DataPosition,
                                    time),
                                currentTruelyFoundCommandPart);
                        }
                        else
                        {
                            // Если найден ответ - проверить, является ли ответ ответом на пердыдущий запрос, или это какой-то другой ответ:
                            if (isCurrentTruelyFoundCommandPartRepliedOnPrevRequest)
                            {
                                // Является, нужно сделать одну целую запись:
                                result.Add(new PsnMapRecord(
                                    prevRequestSubrec.CommandPart.CommandId,
                                    prevRequestSubrec,
                                    new PsnMapSubrecord(
                                        new PsnCommandPartLocation(
                                            currPtLocInfo.DataPosition,
                                            time),
                                        currentTruelyFoundCommandPart)));
                                prevRequestSubrec = null;
                            }
                            else
                            {
                                // Не является, нужно сделать две записи:
                                // (1) запрос
                                if (prevRequestSubrec != null)
                                {
                                    result.Add(new PsnMapRecord(
                                        prevRequestSubrec.CommandPart.CommandId,
                                        prevRequestSubrec,
                                        null));
                                    prevRequestSubrec = null; // Обнуляем запрос после добавления в карту
                                }

                                // (2) ответ
                                result.Add(
                                    new PsnMapRecord(
                                        currentTruelyFoundCommandPart.CommandId,
                                        null,
                                        new PsnMapSubrecord(
                                            new PsnCommandPartLocation(
                                                currPtLocInfo.DataPosition,
                                                time),
                                            currentTruelyFoundCommandPart)
                                    ));
                            }
                        }

                        return partsPosition.CrcOkPartLocationInfo
                            .Length; // Следующий поиск будем проводить с позиции конца хорошей части команды
                    }
                    catch (OutOfMemoryException ex)
                    {
                        throw new Exception("В системе не достаточно памяти для построения командной карты лога", ex);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            "Ошибка алгоритма постоения командной карты лога, обратитесь к разработчикам программы",
                            ex);
                    }
                });
            return result;
        }

        public List<DataPoint> GetDataPoints(IPsnProtocolConfiguration protocol,
            IPsnProtocolCommandPartConfiguration commandPart, IPsnProtocolParameterConfiguration paramInfo,
            DateTime beginTime)
        {
            // Если конфигурация была изменена:
            if (_lastPsnProtocolId != null && _lastPsnProtocolId.IdentyString != protocol.Information.Id.IdentyString)
            {
                FreeResources();
            }

            if (_areResourcesFree)
            {
                FillResources(protocol, beginTime, protocol.CycleCommandParts, protocol.CommandParts);
            }

            var result = new List<DataPoint>();

            bool isWatchForRequestNeeded;
            Func<IPsnMapRecord, IPsnMapSubrecord> funcGetPart;

            if (commandPart.PartType == PsnProtocolCommandPartType.Request)
            {
                funcGetPart = FuncGetPartRequest;
                isWatchForRequestNeeded = false;
            }
            else if (commandPart.PartType == PsnProtocolCommandPartType.Reply)
            {
                funcGetPart = FuncGetPartReply;
                isWatchForRequestNeeded = false;
            }
            else /*if (commandPart.PartType == PsnProtocolCommandPartType.ReplyWithRequiredRequest)*/
            {
                funcGetPart = FuncGetPartReply;
                isWatchForRequestNeeded = true;
            }

            // поток данных нужен для извлечения самих данных (т.к. изначально их нет в карте (есть только информация о местоположении и времени команд протокола ПСН)
            // можно разделить алгоритм на две части: выборку из карты инфомрации об интересующем сигнале, а затем - заполнение значений из потока данных
            for (int i = 0; i < _commandsMap.Count; ++i)
            {
                if (_commandsMap[i].CommandId.IdentyString == commandPart.CommandId.IdentyString)
                {
                    IPsnMapSubrecord needRec = funcGetPart.Invoke(_commandsMap[i]);

                    if (needRec != null)
                    {
                        if (isWatchForRequestNeeded && _commandsMap[i].RequestRecordInfo != null ||
                            !isWatchForRequestNeeded)
                        {
                            // CRC ok
                            int positionInGoodData = needRec.LocationInfo.Position;
                            result.Add(new DataPoint(paramInfo.GetValue(_goodPagesOnlyGoodBytes, positionInGoodData),
                                needRec.LocationInfo.Time, true, positionInGoodData));
                        }
                    }
                }
            }

            return result;
        }


        public void GetDataPoints(IPsnProtocolConfiguration protocol, IPsnProtocolCommandPartConfiguration commandPart,
            IPsnProtocolParameterConfiguration paramInfo, DateTime beginTime, Action<DataPoint> nextPointLoaded)
        {
            // Если конфигурация была изменена:
            if (_lastPsnProtocolId != null && _lastPsnProtocolId.IdentyString != protocol.Information.Id.IdentyString)
            {
                FreeResources();
            }

            if (_areResourcesFree)
            {
                FillResources(protocol, beginTime, protocol.CycleCommandParts, protocol.CommandParts);
            }

            bool isWatchForRequestNeeded;
            Func<IPsnMapRecord, IPsnMapSubrecord> funcGetPart;

            if (commandPart.PartType == PsnProtocolCommandPartType.Request)
            {
                funcGetPart = FuncGetPartRequest;
                isWatchForRequestNeeded = false;
            }
            else if (commandPart.PartType == PsnProtocolCommandPartType.Reply)
            {
                funcGetPart = FuncGetPartReply;
                isWatchForRequestNeeded = false;
            }
            else /*if (commandPart.PartType == PsnProtocolCommandPartType.ReplyWithRequiredRequest)*/
            {
                funcGetPart = FuncGetPartReply;
                isWatchForRequestNeeded = true;
            }

            // поток данных нужен для извлечения самих данных (т.к. изначально их нет в карте (есть только информация о местоположении и времени команд протокола ПСН)
            // можно разделить алгоритм на две части: выборку из карты инфомрации об интересующем сигнале, а затем - заполнение значений из потока данных
            for (int i = 0; i < _commandsMap.Count; ++i)
            {
                if (_commandsMap[i].CommandId.IdentyString == commandPart.CommandId.IdentyString)
                {
                    IPsnMapSubrecord needRec = funcGetPart.Invoke(_commandsMap[i]);

                    if (needRec != null)
                    {
                        if (isWatchForRequestNeeded && _commandsMap[i].RequestRecordInfo != null ||
                            !isWatchForRequestNeeded)
                        {
                            // CRC ok
                            int positionInGoodData = needRec.LocationInfo.Position;
                            nextPointLoaded.Invoke(new DataPoint(
                                paramInfo.GetValue(_goodPagesOnlyGoodBytes, positionInGoodData),
                                needRec.LocationInfo.Time, true, positionInGoodData));
                        }
                    }
                }
            }
        }


        public IEnumerable<IPsnPageIndexRecord> GetPagesIndex()
        {
            if (_areResourcesFree) throw new Exception("FillResources() method must be call first");
            return _pagesIndex;
        }

        public byte[] GetCommandPartBytes(int positionInData)
        {
            for (int i = 0; i < _commandsMap.Count; ++i)
            {
                var cmdPart = _commandsMap[i];
                
                if (cmdPart.ReplyRecordInfo !=null && cmdPart.ReplyRecordInfo.LocationInfo.Position == positionInData)
                    return CopyBytesFromGoodData(positionInData, cmdPart.ReplyRecordInfo.CommandPart.Length);
                
                if (cmdPart.RequestRecordInfo !=null && cmdPart.RequestRecordInfo.LocationInfo.Position == positionInData)
                    return CopyBytesFromGoodData(positionInData, cmdPart.RequestRecordInfo.CommandPart.Length);
            }

            throw new Exception("Cannot find cmd part in position " + positionInData);
        }

        private byte[] CopyBytesFromGoodData(int from, int count)
        {
            var resultBytes = new byte[count];
            for (int j = 0; j < count; ++j)
            {
                resultBytes[j] = _goodPagesOnlyGoodBytes[from + j];
            }

            return resultBytes;
        }
    }
}