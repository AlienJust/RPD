#����������� ��� RPD. ����������� ���������� �� ������ ����������� � ���������� FILES_DIR.
#RELEASE_NO ������ ��������� � ��������� ������ � �������� ���������� ��� ��������� �������� �����.
#***********************************************************************************
;!define RELEASE_NO "19" ;����� ������
#***********************************************************************************
;!define VERSION		"0.9.23.7"			;������ ������ ������������
!define VERSION $%RPDVER%
!define FILES_DIR	"..\..\releases\${VERSION}"	;���� � ����� �� ������� ����� ���������� �����������

#***********************************************************************************
!include MUI2.nsh
#***********************************************************************************

# ��������� �� �����: ����� ��� ����� ���������� ������� ���������� (Var) ����� ������ $Var_Name.
# ��� ��� ����� ���������� ��������, ������������ ��� ������ !define, ����� ������ ${Var_Name}.

Name "���"

!define REGISTRY_DIR "Software\��� ��������\RPD.Presentation" 
!define REGISTRY_UNINSTALLER_DIR "Software\Microsoft\Windows\CurrentVersion\Uninstall\���"
!define UNINSTALLER_FILE "rpd_uninstall.exe"
!define SMPROGRAMS_DIR "$SMPROGRAMS\��� ��������\���"

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
VIAddVersionKey /LANG=${LANG_RUSSIAN} "ProductName" "���"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "Comments" ""
VIAddVersionKey /LANG=${LANG_RUSSIAN} "CompanyName" "��� ��� ��������"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "LegalTrademarks" ""
VIAddVersionKey /LANG=${LANG_RUSSIAN} "LegalCopyright" ""
VIAddVersionKey /LANG=${LANG_RUSSIAN} "FileDescription" "���"
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

#����� ��� ������ �����-�� ������������ ��������. �� ������� ����� �� ����������.
Function CheckVersion
FunctionEnd

Section "-����� � ��������� �����" SecSysFiles
	;��������� � ������ ���� ���������
	WriteRegStr HKLM "${REGISTRY_DIR}" "Install_Dir" "$INSTDIR"

	;���������� � ������ ������ � �������������
	WriteRegStr HKLM "${REGISTRY_UNINSTALLER_DIR}" "DisplayName" "���"
	WriteRegStr HKLM "${REGISTRY_UNINSTALLER_DIR}" "UninstallString" "$INSTDIR\${UNINSTALLER_FILE}"

	;������������
	SetOverwrite on
	SetOutPath "$INSTDIR"
	WriteUninstaller "${UNINSTALLER_FILE}"
	
	;������
	WriteINIStr "$INSTDIR\version.txt" "RPD" "version" "${VERSION}"
	CreateDirectory "${SMPROGRAMS_DIR}"
	CreateShortCut  "${SMPROGRAMS_DIR}\���.lnk" "$INSTDIR\RPD.exe"
	CreateShortCut  "${SMPROGRAMS_DIR}\������� ������ ���.lnk" "$INSTDIR\RPD.Exporter.exe"
	CreateShortCut  "${SMPROGRAMS_DIR}\������� ���.lnk" "$INSTDIR\${UNINSTALLER_FILE}"
SectionEnd

Section "�������� ����� ���������" SecMain
	SetOutPath "$INSTDIR"
	SetOverwrite on
	
	#��������� ���������� �� ���� ���������
	IfFileExists "$INSTDIR\RPD.exe" labExists labOverwrite
	
	labExists:
		MessageBox MB_YESNO  "����� $INSTDIR �� ������. �������� ������������ �����?" IDYES labOverwrite IDNO labNext
	labOverwrite:
	File /r /x RPD.${VERSION}.installer.exe "${FILES_DIR}\*.*"
	
	delete  "$INSTDIR\*.vshost.*"
	delete  "$INSTDIR\*.pdb"
	
	labNext:	

	Exec '"notepad.exe" $INSTDIR\history.txt'
SectionEnd


UninstallText "�������� ���"

Section "Uninstall"
	; ������� ��
	Delete "$INSTDIR\${UNINSTALLER_FILE}" ; delete self
	RMDir /r "$INSTDIR"

	SetShellVarContext all
	RMDir /r "${SMPROGRAMS_DIR}"

	DeleteRegKey HKLM "${REGISTRY_DIR}"
	DeleteRegKey HKLM "${REGISTRY_UNINSTALLER_DIR}"
SectionEnd


LangString DESC_SecLog ${LANG_RUSSIAN} \
"��������� ��� ��������� ������� SCADA. ��� ������ ��������� ��������� ��������� Microsoft .Net Framework 2.0"

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${DESC_SecLog} $(DESC_DESC_SecLog)
!insertmacro MUI_FUNCTION_DESCRIPTION_END
