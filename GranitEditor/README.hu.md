# GranitEditor
[GRÁNIT Bank](https://granitbank.hu/)-os tranzakció csomag generátor.

![image](https://github.com/mattia72/GranitEditor/blob/screenshots/Screenshots/GranitEditor.png "Screenshot"  ) 

A generált xml fájl feltölthető a [netbankban](https://netbank.granitbank.hu) a *Csomag importálása* menüpontban:
* **Importálandó fájl:** a generált fájl elérési útja 
* **Import formátum:** _XML formátum (*.XML)_

![image](https://github.com/mattia72/GranitEditor/blob/screenshots/Screenshots/CsomagImport.png "Screenshot" )

( **FIGYELEM:** A program NEM a Gránit Bank fejlesztése! A használatból eredő károkért a fejlesztő(k) nem vonható(ak) felelősségre! Lsd: [Licenc](LICENSE) )

## Az első lépések

### Telepítés
1. Töltse le a [legfrissebb](https://github.com/mattia72/GranitEditor/releases/latest) installert (`GranitEditorInstaller.msi`)!
1. Kattintson duplán a `GranitEditorInstaller.msi` fájlra a telepítés megkezdéséhez! (Új verzió telepítése automatikusan felülírja a régit!)

### Zip csomag
Ha nem akar telepíteni
1. Töltse le a [legfrissebb](https://github.com/mattia72/GranitEditor/releases/latest) zip fájlt (`GranitEditor.zip`)!
1. Csomagolja ki egy tetszőleges könyvtárba!
1. Indítsa el a `GranitEditor.exe`-t!

### Jellemzők
1. Copy&Paste: Sorokat, cellákat másolhat a megszokott módon.
1. Drag&Drop: Fájlokat dobhat az alkalmazásra editálás céljából.
1. Find&Replace: Keresés és csere, akár reguláris kifejezésekkel is.
1. A táblázat sorai rendezhetőek oszlopok szerint.
1. Utojára megnyitott fájlok listázva vannak a _Fájl_ menüben.
1. Csak a kiválasztott sorok kerülnek mentésre.
1. Validálás: Rossz formátumban nem menthető el a fájl.

## Közreműködés
### Build és Teszt
* IDE: Visual Studio 2017 Community Edition
* Installer: WiX Toolset Visual Studio 2017 Extension
* UnitTest: Visual Studio Test Platform 
* CI: https://ci.appveyor.com/project/mattia72/graniteditor

### XML Leírás 
https://netbank.granitbank.hu/eibpublic_granit/help/hu/IMPXML.html

### Hibák, javaslatok
Ha hibát talál, vagy javaslata van, kérem jelezze [itt](https://github.com/mattia72/GranitEditor/issues/new)!
