using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlienJust.Support.IO;
using DataAbstractionLevel.Low.InternalKitchen.Extensions;
using DataAbstractionLevel.Low.PsnData.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.PsnData {
	internal static class PsnBinParser {
		public static List<PsnBinLogAdvancedLocationInfo> ApplyAdvancedFragmentationFixed(string fileName, IList<IPsnPagesLocationInfo> psnLogFragmentInfos) {
			var resultLocations = new List<PsnBinLogAdvancedLocationInfo>();
			var extractor = PsnPageExtractorFactory.Extractor;
			using (var br = new AdvancedBinaryReader(File.OpenRead(fileName), false)) {
				// Сперва выбираем фрагменты с хорошими первыми страницами:
				foreach (var locationInfo in psnLogFragmentInfos) {
					try {
						br.BaseStream.Seek(extractor.PsnPageSize*locationInfo.PagesInterval.Begin, SeekOrigin.Begin);
						var firstPageHeaderBytes = br.ReadBytes(extractor.PsnPageHeaderLength);

						br.BaseStream.Seek(extractor.PsnPageSize*locationInfo.PagesInterval.End, SeekOrigin.Begin);
						var lastPageHeaderBytes = br.ReadBytes(extractor.PsnPageHeaderLength);


						var firstHeader = extractor.GetHeaderFromRealDevice(firstPageHeaderBytes);
						var lastHeader = extractor.GetHeaderFromRealDevice(lastPageHeaderBytes);

						if (firstHeader.PageInfo == PsnPageInfo.NormalPage && lastHeader.PageInfo == PsnPageInfo.NormalPage) {
							if (locationInfo.TimesInterval.Begin.HasValue && firstHeader.CreatedAt.HasValue) {
								//if (locationInfo.TimesInterval.Begin.Value.EqualToEvenSecond(firstHeader.CreatedAt.Value)) {
								if (lastHeader.CreatedAt.HasValue)
									if (lastHeader.CreatedAt.Value > firstHeader.CreatedAt.Value) {
										resultLocations.Add(
											new PsnBinLogAdvancedLocationInfo(
												fileName,
												new PsnPageNumberAndTime(locationInfo.PagesInterval.Begin, firstHeader.CreatedAt.Value),
												new PsnPageNumberAndTime(locationInfo.PagesInterval.End, lastHeader.CreatedAt.Value)));
									}
							}
						}
					}
					catch {
						// TODO: remove empty catch;
					}
				}
				// Сортировка фрагментов по дате и указание последней записи
				if (resultLocations.Count > 1) {
					var orderedLocations = resultLocations.OrderBy(loc => loc.FirstPageInfo.Time.Value).ToList();
					var lastLocation = orderedLocations.Last();
					orderedLocations.RemoveAt(orderedLocations.Count - 1);
					orderedLocations.Add(new PsnBinLogAdvancedLocationInfo(fileName, lastLocation.FirstPageInfo, lastLocation.LastPageInfo) {IsLastPsnLogOnDevice = true});
					resultLocations = orderedLocations;
				}

				return resultLocations;
			}
		}

		public static List<PsnBinLogAdvancedLocationInfo> ApplyAdvancedFragmentationStack(string fileName, IList<IPsnPagesLocationInfo> psnLogFragmentInfos) {
			var resultLocations = new List<PsnBinLogAdvancedLocationInfo>();
			var extractor = PsnPageExtractorFactory.Extractor;
			using (var br = new AdvancedBinaryReader(File.OpenRead(fileName), false)) {
				// Сперва выбираем фрагменты с хорошими первыми страницами:
				foreach (var locationInfo in psnLogFragmentInfos) {
					try {
						br.BaseStream.Seek(extractor.PsnPageSize*locationInfo.PagesInterval.Begin, SeekOrigin.Begin);
						var firstPageHeaderBytes = br.ReadBytes(extractor.PsnPageHeaderLength);

						br.BaseStream.Seek(extractor.PsnPageSize*locationInfo.PagesInterval.End, SeekOrigin.Begin);
						var lastPageHeaderBytes = br.ReadBytes(extractor.PsnPageHeaderLength);


						var firstHeader = extractor.GetHeaderFromRealDevice(firstPageHeaderBytes);
						var lastHeader = extractor.GetHeaderFromRealDevice(lastPageHeaderBytes);

						if (firstHeader.PageInfo == PsnPageInfo.NormalPage && lastHeader.PageInfo == PsnPageInfo.NormalPage) {
							if (locationInfo.TimesInterval.Begin.HasValue && firstHeader.CreatedAt.HasValue) {
								if (locationInfo.TimesInterval.Begin.Value.EqualToEvenSecond(firstHeader.CreatedAt.Value)) {
									if (lastHeader.CreatedAt.HasValue)
										if (lastHeader.CreatedAt.Value > firstHeader.CreatedAt.Value) {
											resultLocations.Add(
												new PsnBinLogAdvancedLocationInfo(
													fileName,
													new PsnPageNumberAndTime(locationInfo.PagesInterval.Begin, firstHeader.CreatedAt.Value),
													new PsnPageNumberAndTime(locationInfo.PagesInterval.End, lastHeader.CreatedAt.Value)));
										}
								}
							}
						}
					}
					catch (Exception ex){
						// TODO: remove empty catch
						Console.Write(ex);
					}
				}
				// Сортировка фрагментов по дате и указание последней записи
				if (resultLocations.Count > 1) {
					var orderedLocations = resultLocations.OrderBy(loc => loc.FirstPageInfo.Time.Value).ToList();
					var lastLocation = orderedLocations.Last();
					orderedLocations.RemoveAt(orderedLocations.Count - 1);
					orderedLocations.Add(new PsnBinLogAdvancedLocationInfo(fileName, lastLocation.FirstPageInfo, lastLocation.LastPageInfo) {IsLastPsnLogOnDevice = true});
					resultLocations = orderedLocations;
				}

				return resultLocations;
			}
		}

		public static PsnBinLogAdvancedLocationInfo GetInfoFromFragment(string fileName) {
			//var result = new List<PsnBinLogAdvancedLocationInfo();
			var fileInfo = new FileInfo(fileName);

			var extractor = PsnPageExtractorFactory.Extractor;
			var pagesCount = fileInfo.Length/extractor.PsnPageSize;

			var firstDatedPageInfo = GetFirstPageNumberWithGoodDate(new PsnBinLogLocationInfo(fileName, 0, (int) (pagesCount - 1)), extractor.GetHeaderFromRealDevice);
			var lastDatedPageInfo = GetLastPageNumberWithGoodDate(new PsnBinLogLocationInfo(fileName, 0, (int) (pagesCount - 1)), extractor.GetHeaderFromRealDevice);

			return new PsnBinLogAdvancedLocationInfo(fileName, firstDatedPageInfo, lastDatedPageInfo);
		}

		public static PsnBinLogAdvancedLocationInfo GetInfoFromSavedFragment(string fileName) {
			var fileInfo = new FileInfo(fileName);

			var extractor = PsnPageExtractorFactory.Extractor;
			var pagesCount = fileInfo.Length/extractor.PsnPageSize;

			var firstDatedPageInfo = GetFirstPageNumberWithGoodDate(new PsnBinLogLocationInfo(fileName, 0, (int) (pagesCount - 1)), extractor.GetHeaderFromRealDevice);
			var lastDatedPageInfo = GetLastPageNumberWithGoodDate(new PsnBinLogLocationInfo(fileName, 0, (int) (pagesCount - 1)), extractor.GetHeaderFromRealDevice);

			return new PsnBinLogAdvancedLocationInfo(fileName, firstDatedPageInfo, lastDatedPageInfo);
		}

		public static IList<IPsnPagesLocationInfo> GetPsnPagesIntervalsFixed(string fileName, IList<IPsnDataFragmentInformation> psnLogFragmentInfos, int lastWrittenPageGlobalIndex, int psnFileStartPageGlobalIndex) {
			// [-----|-----|-----|-----|--o--|-----|-----|-----|-----|-----|-----]
			// o - is lastWrittenPage
			// [ - is psnFileStartPageGlobalIndex
			// ] - is psnFileStartPageGlobalIndex + lastPsnPageIndex

			var result = new List<IPsnPagesLocationInfo>();
			var extractor = PsnPageExtractorFactory.Extractor;
			var psnFilePagesCount = (int) new FileInfo(fileName).Length/extractor.PsnPageSize;
			var lastPsnPageIndex = psnFilePagesCount - 1; // Индекс последней страницы относительно начала файла PSN.BIN
			for (int i = 0; i < psnLogFragmentInfos.Count; ++i) {
				var fragmentInfoCurr = psnLogFragmentInfos[i];

				var firstPageOfTheFragment = fragmentInfoCurr.StartOffset;
				// Конечная страница лога выбирается так: либо это предыдущая начальной странице следующего лога страница, либо это конец файла PSN.bin
				// TODO: bad code, too much conditional code
				var lastPageOfTheFragment = (i < psnLogFragmentInfos.Count - 1) ? (psnLogFragmentInfos[i + 1].StartOffset - 1) : (psnFileStartPageGlobalIndex + lastPsnPageIndex);
				var shouldSplitLog = firstPageOfTheFragment < lastWrittenPageGlobalIndex && lastWrittenPageGlobalIndex < lastPageOfTheFragment;

				// TODO: analyze apply changes from branch PsnFragmentsEndsFixes
				// TODO: test and debug on real data from 2013.12.09 (fixed logs cound must be 1, power on logs count must be 2)
				if (!shouldSplitLog) {
					result.Add(new SimplePsnPagesLocationInfo {
						PagesInterval = new SimlpleInterval<int> {Begin = firstPageOfTheFragment - psnFileStartPageGlobalIndex, End = lastPageOfTheFragment - psnFileStartPageGlobalIndex},
						TimesInterval = new SimlpleInterval<DateTime?> {Begin = fragmentInfoCurr.StartTime, End = null},
						SplitInfo = FragmentSplitInfo.NotSplitted
					});
				}
				else {
					result.Add(new SimplePsnPagesLocationInfo {
						PagesInterval = new SimlpleInterval<int> {Begin = firstPageOfTheFragment - psnFileStartPageGlobalIndex, End = lastWrittenPageGlobalIndex - psnFileStartPageGlobalIndex - 1},
						TimesInterval = new SimlpleInterval<DateTime?> {Begin = fragmentInfoCurr.StartTime, End = null},
						SplitInfo = FragmentSplitInfo.SplittedFirst
					});
					// Про этот лог мало чего известно (это остаток записи с прошлого цикла, возможно его и не будет (FFFFFFFFFFFFFFFFFFF)
					result.Add(new SimplePsnPagesLocationInfo {
						PagesInterval = new SimlpleInterval<int> {Begin = lastWrittenPageGlobalIndex - psnFileStartPageGlobalIndex, End = lastPageOfTheFragment - psnFileStartPageGlobalIndex},
						TimesInterval = new SimlpleInterval<DateTime?> {Begin = null, End = null},
						SplitInfo = FragmentSplitInfo.SplittedLast
					});
				}

			}
			return result;
		}

		public static IList<IPsnPagesLocationInfo> GetPsnPagesIntervalsStack(string fileName, IList<IPsnDataFragmentInformation> psnLogFragmentInfos, int lastWrittenPageGlobalIndex, int psnFileStartPageGlobalIndex) {
			var result = new List<IPsnPagesLocationInfo>();
			var extractor = PsnPageExtractorFactory.Extractor;
			var psnFilePagesCount = (int) new FileInfo(fileName).Length/extractor.PsnPageSize;
			var lastPsnPageIndex = psnFilePagesCount - 1; // Индекс последней страницы относительно начала файла PSN.BIN
			for (int i = 0; i < psnLogFragmentInfos.Count; ++i) {
				var fragmentInfoCurr = psnLogFragmentInfos[i];

				var firstPageOfTheFragment = (int) fragmentInfoCurr.StartOffset;
				// Конечная страница лога выбирается так: либо это предыдущая начальной странице следующего лога страница, либо это последняя записанная страница
				// TODO: bad code, too much conditional code
				var lastPageOfTheFragment = i < psnLogFragmentInfos.Count - 1 ? psnLogFragmentInfos[i + 1].StartOffset - 1 : lastWrittenPageGlobalIndex;
				var shouldSplitLog = lastPageOfTheFragment < firstPageOfTheFragment;

				if (!shouldSplitLog) {
					result.Add(new SimplePsnPagesLocationInfo {
						PagesInterval = new SimlpleInterval<int> {Begin = firstPageOfTheFragment - psnFileStartPageGlobalIndex, End = lastPageOfTheFragment - psnFileStartPageGlobalIndex - 1},
						TimesInterval = new SimlpleInterval<DateTime?> {Begin = fragmentInfoCurr.StartTime, End = null},
						SplitInfo = FragmentSplitInfo.NotSplitted
					});
				}
				else {
					// эта ветка ветвления срабатывает, когда лог переходит границу
					result.Add(new SimplePsnPagesLocationInfo {
						PagesInterval = new SimlpleInterval<int> {Begin = firstPageOfTheFragment - psnFileStartPageGlobalIndex, End = lastPsnPageIndex},
						TimesInterval = new SimlpleInterval<DateTime?> {Begin = fragmentInfoCurr.StartTime, End = null},
						SplitInfo = FragmentSplitInfo.SplittedFirst
					});

					result.Add(new SimplePsnPagesLocationInfo {
						PagesInterval = new SimlpleInterval<int> {Begin = 0, End = lastPageOfTheFragment - psnFileStartPageGlobalIndex - 1},
						TimesInterval = new SimlpleInterval<DateTime?> {Begin = null, End = null},
						SplitInfo = FragmentSplitInfo.SplittedLast
					});
				}

			}
			return result;
		}

		private static PsnPageNumberAndTime GetFirstPageNumberWithGoodDate(PsnBinLogLocationInfo locationInfo, Func<byte[], PsnPageHeader> timeReceiveMethod) {
			using (var br = new AdvancedBinaryReader(File.OpenRead(locationInfo.PsnBinFileName), false)) {
				var extractor = PsnPageExtractorFactory.Extractor;
				var buf = new byte[extractor.PsnPageHeaderLength];
				for (int i = locationInfo.FirstPageIndex; i <= locationInfo.LastPageIndex; ++i) {
					try {
						br.BaseStream.Seek(extractor.PsnPageSize*i, SeekOrigin.Begin);
						br.Read(buf, 0, buf.Length);
						var header = timeReceiveMethod(buf); //PsnBinPageExtractor.GetHeaderFromRealDevice(buf);
						br.Close();
						return new PsnPageNumberAndTime(i, header.CreatedAt);
					}
					catch {
						// в случае ошибки цикл продолжается, т.к. нужная страница не найдена
					}
				}
			}
			throw new Exception("Cannot find any dated page date");
		}

		private static PsnPageNumberAndTime GetLastPageNumberWithGoodDate(PsnBinLogLocationInfo locationInfo, Func<byte[], PsnPageHeader> timeReceiveMethod) {
			using (var br = new AdvancedBinaryReader(File.OpenRead(locationInfo.PsnBinFileName), false)) {
				var extractor = PsnPageExtractorFactory.Extractor;
				var buf = new byte[extractor.PsnPageHeaderLength];
				for (int i = locationInfo.LastPageIndex; i >= locationInfo.FirstPageIndex; --i) {
					try {
						br.BaseStream.Seek(extractor.PsnPageSize*i, SeekOrigin.Begin);
						br.Read(buf, 0, buf.Length);
						var header = timeReceiveMethod(buf); //PsnBinPageExtractor.GetHeaderFromRealDevice(buf);
						br.Close();
						return new PsnPageNumberAndTime(i, header.CreatedAt);
					}
					catch {
						// в случае ошибки цикл продолжается, т.к. нужная страница не найдена
					}
				}
			}
			throw new Exception("Cannot find any dated page");
		}
	}
}