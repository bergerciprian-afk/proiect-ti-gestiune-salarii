<%@ Page Title="Fluturasi Salarii" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Fluturasi.aspx.cs" Inherits="SalariiApp.Fluturasi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .filter-fluturasi-bar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: #ffffff;
            padding: 10px 14px;
            border: 1px solid var(--rose-border);
            margin-bottom: 15px;
            flex-wrap: wrap;
            gap: 10px;
        }
        .fluturasi-grid-view {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(460px, 1fr));
            gap: 16px;
        }
        .fluturas-item {
            background: #ffffff;
            border: 2px dashed #708090;
            padding: 14px 18px;
            page-break-inside: avoid;
        }
        .fluturas-header {
            display: flex;
            justify-content: space-between;
            border-bottom: 2px solid var(--rose-border);
            padding-bottom: 4px;
            margin-bottom: 10px;
            font-weight: 700;
            color: var(--rose-ink);
        }
        .fluturas-columns {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 14px;
            font-size: 0.85rem;
        }
        .fluturas-row {
            display: flex;
            justify-content: space-between;
            padding: 3px 0;
            border-bottom: 1px dotted #ccc;
        }
        .fluturas-row-highlight {
            background-color: #ffff99;
            font-weight: 700;
            border-top: 1px solid #444;
            border-bottom: 1px solid #444;
            padding: 4px 6px;
            margin-top: 6px;
            color: #000000;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header no-print">
        <h2 class="page-title">Fluturasi Salarii</h2>
    </div>

    <div class="filter-fluturasi-bar no-print">
        <div style="display: flex; gap: 10px; align-items: center;">
            <label style="font-weight: 600;">Filtrare angajat:</label>
            <asp:DropDownList ID="ddlAngajati" runat="server" CssClass="form-control" 
                AutoPostBack="true" OnSelectedIndexChanged="ddlAngajati_SelectedIndexChanged" />
            <asp:Button ID="btnArataToti" runat="server" Text="Arata toti" CssClass="btn btn-outline" OnClick="btnArataToti_Click" />
        </div>
        <div>
            <button type="button" class="btn btn-primary" onclick="window.print();">
                Tipareste fluturasi
            </button>
        </div>
    </div>

    <div class="fluturasi-container fluturasi-grid-view">
        <asp:Repeater ID="rptFluturasi" runat="server">
            <ItemTemplate>
                <div class="fluturas-card fluturas-item">
                    <div class="fluturas-header">
                        <span>FLUTURAS SALARIU</span>
                        <span>DATA: <%# Eval("DATA_CURENTA") %></span>
                    </div>

                    <div class="fluturas-columns">
                        <div>
                            <div class="fluturas-row">
                                <span>NR. CRT:</span>
                                <strong><%# Eval("NR_CRT") %></strong>
                            </div>
                            <div class="fluturas-row">
                                <span>NUME:</span>
                                <strong><%# Eval("NUME") %></strong>
                            </div>
                            <div class="fluturas-row">
                                <span>PRENUME:</span>
                                <strong><%# Eval("PRENUME") %></strong>
                            </div>
                            <div class="fluturas-row">
                                <span>FUNCTIE:</span>
                                <span><%# Eval("FUNCTIE") %></span>
                            </div>
                            <div class="fluturas-row">
                                <span>SALAR BAZA:</span>
                                <span><%# Eval("SALAR_BAZA", "{0:N0}") %> RON</span>
                            </div>
                            <div class="fluturas-row">
                                <span>SPOR %:</span>
                                <span><%# Eval("SPOR_PROCENT") %>%</span>
                            </div>
                            <div class="fluturas-row">
                                <span>PREMII BRUTE:</span>
                                <span><%# Eval("PREMII_BRUTE", "{0:N0}") %> RON</span>
                            </div>
                            <div class="fluturas-row" style="background: #fefce8; font-weight: bold;">
                                <span>TOTAL BRUT:</span>
                                <span><%# Eval("TOTAL_BRUT", "{0:N0}") %> RON</span>
                            </div>
                            <div class="fluturas-row">
                                <span>BRUT IMPOZABIL:</span>
                                <span><%# Eval("BRUT_IMPOZABIL", "{0:N0}") %> RON</span>
                            </div>
                        </div>

                        <div>
                            <div class="fluturas-row">
                                <span>CAS (<%# Eval("CAS_PROC") %>%):</span>
                                <span><%# Eval("CAS", "{0:N0}") %> RON</span>
                            </div>
                            <div class="fluturas-row">
                                <span>CASS (<%# Eval("CASS_PROC") %>%):</span>
                                <span><%# Eval("CASS", "{0:N0}") %> RON</span>
                            </div>
                            <div class="fluturas-row">
                                <span>IMPOZIT (<%# Eval("IMP_PROC") %>%):</span>
                                <span><%# Eval("IMPOZIT", "{0:N0}") %> RON</span>
                            </div>
                            <div class="fluturas-row">
                                <span>RETINERI:</span>
                                <span><%# Eval("RETINERI", "{0:N0}") %> RON</span>
                            </div>
                            
                            <div class="fluturas-row fluturas-row-highlight" style="margin-top: 24px;">
                                <span style="color: #166534;">VIRAT CARD:</span>
                                <span style="color: #15803d; font-size: 1.05rem;"><%# Eval("VIRAT_CARD", "{0:N0}") %> RON</span>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
