#Инсталлятор для RPD. Инсталлятор собирается из файлов находящихся в директории FILES_DIR.
#RELEASE_NO должен совпадать с последней цифрой в названии директории где находятся исходные файлы.
#***********************************************************************************
;!define RELEASE_NO "19" ;Номер релиза
#***********************************************************************************
;!define VERSION		"0.9.23.7"			;Полная версия инсталлятора
!define VERSION $%RPDVER%
!define FILES_DIR	"..\..\releases\${VERSION}"	;Путь к папке из которой будет собираться инсталлятор

#***********************************************************************************
!include MUI2.nsh
#***********************************************************************************

# Пояснение по языку: Везде где нужно подставить зачение переменной (Var) нужно писать $Var_Name.
# Там где нужно подставить значение, определенное при помощи !define, нужно писать ${Var_Name}.

Name "РПД"

!define REGISTRY_DIR "Software\НПП Горизонт\RPD.Presentation" 
!define REGISTRY_UNINSTALLER_DIR "Software\Microsoft\Windows\CurrentVersion\Uninstall\РПД"
!define UNINSTALLER_FILE "rpd_uninstall.exe"
!define SMPROGRAMS_DIR "$SMPROGRAMS\НПП Горизонт\РПД"

InstallDir "$PROGRAMFILES\HORIZONT\RPD.Presentation"
InstallDirRegKey HKLM "${REGISTRY_DIR}" "Install_Dir"

SetDatablockOptimize on
SetCompressor /SOLID lzma
SetCompress force
AllowRootDirInstall false
ShowInstDetails show
XPStyle on
SetDateSave on

OutFile "${FILES_DIR}\RPD.${VERSION}.installer.exe"

!insertmacro MUI_LANGUAGE "Russian"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "ProductName" "РПД"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "Comments" ""
VIAddVersionKey /LANG=${LANG_RUSSIAN} "CompanyName" "ООО НПП ГОРИЗОНТ"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "LegalTrademarks" ""
VIAddVersionKey /LANG=${LANG_RUSSIAN} "LegalCopyright" ""
VIAddVersionKey /LANG=${LANG_RUSSIAN} "FileDescription" "РПД"
VIProductVersion "${VERSION}"


!define MUI_PAGE_CUSTOMFUNCTION_LEAVE CheckVersion
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!define DOT_MAJOR "4"
!define DOT_MINOR "0"
!define DOT_MINOR_MINOR "30319"

#Нужна для работы каких-то подключаемых макросов. Не убирать иначе всё поломается.
Function CheckVersion
FunctionEnd

Section "-Общие и системные файлы" SecSysFiles
	;сохраняем в реестр путь установки
	WriteRegStr HKLM "${REGISTRY_DIR}" "Install_Dir" "$INSTDIR"

	;записываем в реестр данные о деинсталяторе
	WriteRegStr HKLM "${REGISTRY_UNINSTALLER_DIR}" "DisplayName" "РПД"
	WriteRegStr HKLM "${REGISTRY_UNINSTALLER_DIR}" "UninstallString" "$INSTDIR\${UNINSTALLER_FILE}"

	;деинсталятор
	SetOverwrite on
	SetOutPath "$INSTDIR"
	WriteUninstaller "${UNINSTALLER_FILE}"
	
	;версия
	WriteINIStr "$INSTDIR\version.txt" "RPD" "version" "${VERSION}"
	CreateDirectory "${SMPROGRAMS_DIR}"
	CreateShortCut  "${SMPROGRAMS_DIR}\РПД.lnk" "$INSTDIR\RPD.exe"
	CreateShortCut  "${SMPROGRAMS_DIR}\Экспорт данных РПД.lnk" "$INSTDIR\RPD.Exporter.exe"
	CreateShortCut  "${SMPROGRAMS_DIR}\Удалить РПД.lnk" "$INSTDIR\${UNINSTALLER_FILE}"
SectionEnd

Section "Основные файлы программы" SecMain
	SetOutPath "$INSTDIR"
	SetOverwrite on
	
	#Проверяем существуют ли файл программы
	IfFileExists "$INSTDIR\RPD.exe" labExists labOverwrite
	
	labExists:
		MessageBox MB_YESNO  "Папка $INSTDIR не пустая. Заменить существующие файлы?" IDYES labOverwrite IDNO labNext
	labOverwrite:
	File /r /x RPD.${VERSION}.installer.exe "${FILES_DIR}\*.*"
	
	delete  "$INSTDIR\*.vshost.*"
	delete  "$INSTDIR\*.pdb"
	
	labNext:	

	Exec '"notepad.exe" $INSTDIR\history.txt'
SectionEnd


UninstallText "Удаление РПД"

Section "Uninstall"
	; удаляем всё
	Delete "$INSTDIR\${UNINSTALLER_FILE}" ; delete self
	RMDir /r "$INSTDIR"

	SetShellVarContext all
	RMDir /r "${SMPROGRAMS_DIR}"

	DeleteRegKey HKLM "${REGISTRY_DIR}"
	DeleteRegKey HKLM "${REGISTRY_UNINSTALLER_DIR}"
SectionEnd


LangString DESC_SecLog ${LANG_RUSSIAN} \
"Программа для просмотра журнала SCADA. Для работы требуется отдельная установка Microsoft .Net Framework 2.0"

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${DESC_SecLog} $(DESC_DESC_SecLog)
!insertmacro MUI_FUNCTION_DESCRIPTION_END
