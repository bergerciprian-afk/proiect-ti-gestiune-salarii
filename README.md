# Proiect TI · Gestiune salarii

Aplicație ASP.NET Web Forms pentru evidența angajaților, calculul salariilor, statul de plată și fluturași. Baza Oracle se creează local cu opt angajați fictivi.

## 1. Instalează ce îți trebuie

- Pe Windows, instalează Visual Studio Community cu workload-ul **ASP.NET and web development**, **IIS Express** și instrumentele pentru **.NET Framework 4.8**. [Componente Visual Studio](https://learn.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-community)
- Instalează Oracle Database **XE 21c pentru Windows x64**. Extrage kitul Oracle, rulează `setup.exe` și reține parola aleasă pentru contul Oracle `SYSTEM`. [Descărcare Oracle XE](https://www.oracle.com/database/technologies/xe-downloads.html)

## 2. Pregătește baza de date

Extrage proiectul. În folderul cu fișierul `.sln`, deschide PowerShell și rulează o singură dată:

```powershell
sqlplus -L system@localhost:1521/XEPDB1 '@Database\00_configurare.sql'
```

Introdu parola instalării pentru `SYSTEM`, apoi alege o parolă nouă pentru `TI_SALARII`, formată din cel puțin 12 litere și cifre. Configurarea se încheie cu mesajul `Configurare finalizata pentru TI_SALARII.`

Dacă PowerShell nu găsește `sqlplus`, folosește `sqlplus.exe` din folderul `bin` al instalării Oracle. Dacă conexiunea nu merge, verifică în **Services** că serviciul bazei Oracle XE și listenerul Oracle sunt pornite. [Conectarea la XE](https://docs.oracle.com/en/database/oracle/oracle-database/21/xeinw/connecting-oracle-database-xe.html)

## 3. Pornește aplicația

1. Din același PowerShell, creează fișierul local de conectare.

   ```powershell
   Copy-Item SalariiApp\Oracle.local.config.example SalariiApp\Oracle.local.config
   notepad SalariiApp\Oracle.local.config
   ```

2. În Notepad, înlocuiește `INLOCUIESTE_PAROLA` cu parola aleasă pentru `TI_SALARII` și salvează. Păstrează fișierul `Oracle.local.config` pe calculatorul tău. Git îl ignoră.
3. Deschide `Proiect_TI_Gestiune_Salarii.sln` în Visual Studio. Lasă NuGet să descarce pachetele la prima compilare. În bara de sus selectează **IIS Express** și apasă **Ctrl+F5**.
4. Pe prima pagină trebuie să apară **Conectat la Oracle**. În **Actualizare date** ar trebui să vezi cei opt angajați. Poți demonstra apoi **Calcul salarii**, **Stat plata** și **Fluturasi**.

Parola demonstrativă pentru pagina **Modificare procente** este `DemoTI2026`.
