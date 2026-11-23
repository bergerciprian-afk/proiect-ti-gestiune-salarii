<%@ Page Title="Acasa" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SalariiApp.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .help-container {
            background-color: #ffffff;
            border: 1px solid #c0c0c0;
            padding: 24px 30px;
            font-size: 0.92rem;
            line-height: 1.6;
        }
        .help-section-title {
            color: var(--rose-ink);
            font-size: 1.15rem;
            border-bottom: 2px solid var(--rose-control-border);
            padding-bottom: 4px;
            margin-top: 20px;
            margin-bottom: 12px;
            font-weight: bold;
        }
        .formula-box {
            background-color: var(--rose-bg);
            border: 1px solid var(--rose-border);
            border-left: 5px solid var(--rose-control-border);
            padding: 12px 18px;
            margin: 12px 0;
            font-family: 'Consolas', 'Courier New', monospace;
            font-size: 0.95rem;
            line-height: 1.8;
            color: #1c2833;
        }
        .test-values-table {
            width: 100%;
            max-width: 650px;
            border-collapse: collapse;
            margin: 12px 0 18px;
            font-size: 0.9rem;
        }
        .test-values-table th, .test-values-table td {
            border: 1px solid var(--rose-border);
            padding: 6px 10px;
        }
        .test-values-table th {
            background-color: var(--rose-panel);
            text-align: left;
            font-weight: bold;
        }
        .test-calc {
            background-color: #ffff99;
            font-weight: bold;
        }
        .quick-info-box {
            background-color: var(--rose-soft);
            border: 1px solid #d0dbe5;
            padding: 10px 15px;
            margin-bottom: 18px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            font-size: 0.88rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2 class="page-title">Ghid de Utilizare si Ajutor Aplicatie (Home)</h2>
    </div>

    <div class="quick-info-box">
        <div>
            Status aplicatie: <strong><asp:Label ID="lblStatusAplicatie" runat="server" Text="Se conecteaza la Oracle..." /></strong> | 
            Total angajati inregistrati: <strong><asp:Label ID="lblTotalAngajati" runat="server" Text="0" /></strong>
        </div>
        <div>
            Cote taxe curente: <strong><asp:Label ID="lblTaxeCurenteVizibile" runat="server" Text="-" /></strong>
        </div>
    </div>
    <asp:Label ID="lblEroareConexiune" runat="server" Visible="false" CssClass="alert alert-danger" />

    <div class="help-container">
        <p style="margin-bottom: 14px;">
            Aplicatia gestioneaza evidenta si calculul salariilor pentru angajati, conform cerintelor proiectului de ASP.NET cu baza de date Oracle. 
            Site-ul este structurat pe o pagina Master, include meniu orizontal cu butoane rapide, ceas de server si reclame afisate cu AJAX, 
            precum si proceduri si triggere pentru actualizarea automata a salariilor.
        </p>

        <div class="help-section-title">1. Optiunile din meniu</div>
        <ul style="margin-left: 22px; margin-bottom: 16px;">
            <li style="margin-bottom: 8px;">
                <strong>Home:</strong> Pagina principala cu ghidul de utilizare si formulele de calcul folosite.
            </li>
            <li style="margin-bottom: 8px;">
                <strong>Actualizare date:</strong> 
                Permite cautarea angajatilor dupa nume, prenume sau functie si modificarea salariului de baza, a sporului, a primelor sau a retinerilor. 
                Coloanele calculate sunt afisate pe fundal galben si nu pot fi modificate de la tastatura.
            </li>
            <li style="margin-bottom: 8px;">
                <strong>Adaugare angajati:</strong> 
                Formular pentru introducerea unui angajat nou. Campurile au valori implicite (Spor = 0%, Premii = 0, Retineri = 0) si sunt validate. 
                La salvare, salariul se calculeaza automat pe loc, formularul ramanand deschis pentru o noua inregistrare. Se poate atasa si poza (BLOB).
            </li>
            <li style="margin-bottom: 8px;">
                <strong>Stergere angajati:</strong> 
                Permite cautarea dupa nume sau prenume, selectarea persoanei dorite din lista si stergerea din baza de date dupa confirmare.
            </li>
            <li style="margin-bottom: 8px;">
                <strong>Calcul salarii:</strong> 
                Tabel centralizat cu toti salariatii si valorile calculate, cu optiune de filtrare si buton pentru recalcularea tuturor salariilor.
            </li>
            <li style="margin-bottom: 8px;">
                <strong>Stat de plata:</strong> 
                Genereaza raportul tabelar pe format A3 orizontal (Landscape), cu totaluri generale pe coloanele Total Brut si Virat Card. 
                Permite setarea lunii, completarea observatiilor si tiparirea sau salvarea ca PDF.
            </li>
            <li style="margin-bottom: 8px;">
                <strong>Fluturasi:</strong> 
                Afiseaza fluturasii de salariu pe doua coloane, fie pentru toti salariatii, fie individual prin selectarea unui angajat din lista.
            </li>
            <li style="margin-bottom: 8px;">
                <strong>Modificare procente:</strong> 
                Zona protejata cu parola pentru modificarea cotelor de CAS (25%), CASS (10%) si Impozit (10%). La salvare, salariile se recalculeaza automat.
            </li>
        </ul>

        <div class="help-section-title">2. Formule de calcul</div>
        <p>Toate valorile calculate sunt rotunjite la intreg:</p>
        <div class="formula-box">
            <strong>Calcul venit brut:</strong><br />
            TOTAL_BRUT = SALAR_BAZA * (1 + SPOR_% / 100) + PREMII_BRUTE<br /><br />
            <strong>Contributii si impozit:</strong><br />
            CAS (pensie) = TOTAL_BRUT * CAS_% [implicit 25%]<br />
            CASS (sanatate) = TOTAL_BRUT * CASS_% [implicit 10%]<br />
            BRUT_IMPOZABIL = TOTAL_BRUT - CAS - CASS<br />
            IMPOZIT = BRUT_IMPOZABIL * IMPOZIT_% [implicit 10%]<br /><br />
            <strong>Calcul venit net:</strong><br />
            VIRAT_CARD = TOTAL_BRUT - IMPOZIT - CAS - CASS - RETINERI
        </div>

        <div class="help-section-title">3. Exemplu numeric de verificare (Popescu Ion)</div>
        <table class="test-values-table">
            <thead>
                <tr>
                    <th>Camp</th>
                    <th>Valoare</th>
                    <th>Camp</th>
                    <th>Valoare calculata</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>NUME / PRENUME</td>
                    <td>Popescu Ion</td>
                    <td>TOTAL_BRUT</td>
                    <td class="test-calc">4400 RON</td>
                </tr>
                <tr>
                    <td>FUNCTIE</td>
                    <td>Inginer</td>
                    <td>CAS (25%)</td>
                    <td class="test-calc">1100 RON</td>
                </tr>
                <tr>
                    <td>SALAR_BAZA</td>
                    <td>4000 RON</td>
                    <td>CASS (10%)</td>
                    <td class="test-calc">440 RON</td>
                </tr>
                <tr>
                    <td>SPOR_%</td>
                    <td>10 %</td>
                    <td>BRUT_IMPOZABIL</td>
                    <td class="test-calc">2860 RON</td>
                </tr>
                <tr>
                    <td>PREMII_BRUTE</td>
                    <td>0 RON</td>
                    <td>IMPOZIT (10%)</td>
                    <td class="test-calc">286 RON</td>
                </tr>
                <tr>
                    <td>RETINERI</td>
                    <td>0 RON</td>
                    <td>VIRAT_CARD</td>
                    <td class="test-calc">2574 RON</td>
                </tr>
            </tbody>
        </table>

        <div class="help-section-title">4. Ghid rapid de utilizare</div>
        <p>
            1. In <em>Adaugare angajati</em>, completati datele si apasati <strong>Salveaza angajat</strong> pentru a vedea calculul salariului.<br />
            2. Pentru tiparirea statului de plata sau salvare ca PDF, accesati <em>Stat plata</em> si apasati <strong>"Tipareste (A3 / PDF)"</strong>.<br />
            3. Pentru modificarea cotelor de taxe, introduceti parola administratorului configurata in baza de date.
        </p>
    </div>
</asp:Content>
