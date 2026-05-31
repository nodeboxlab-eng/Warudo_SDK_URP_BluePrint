Warudo_UI_Control_InFolder (단일 소스)
=======================================

고정 역할 ID: Warudo_UI_Control_InFolder — 상세는 이 폴더의 MOD_ROLE.txt

경로(예시): Pro/Lite 폴더 안에 둘 수 있음 — 현재 프로젝트:
  Assets/00.WarudoBuild/BP/My New Mod/Warudo_UI_Control_InFolder/

공용: ThemeMode, AccentColor, UITheme, Events/UIButtonClickEvent

정식(`Warudo_UI_Control`)·Lite(`Warudo_UI_Control_Lite`) 모두 이 폴더만 참조한다.
Lite 폴더 **안**에 InFolder를 또 두지 않는다.

Unity 프로젝트에서는 보통 **InFolder + 정식 + Lite 폴더가 같이** 있다.
(「둘 중 하나만 두기」가 아님 — 개발 시에는 세트 모두 유지.)

uMod (ExportSettings)
---------------------

- **referencePaths 는 비움** — uMod가 경로를 “참조 모드”로 해석하다가 해석 실패하는 경우가 있어, 여기서는 쓰지 않는다.
- InFolder를 **정식 / Lite 각 폴더 안**에 둘지, BP에만 둘지는 **직접 맞춘다.**

Unity 안에서 `UITheme` 등이 **두 경로에 동시에** 있으면 중복 컴파일이 나므로, 프로젝트에는 **한 곳의 InFolder만** 두는 것이 안전하다.
