<%@ Page Title="Stat de Plata" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatPlata.aspx.cs" Inherits="SalariiApp.StatPlata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        @media print {
            @page {
                size: A3 landscape;
                margin: 10mm;
            }
        }

        .report-toolbar {
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
        .stat-plata-sheet {
            background: #ffffff;
            padding: 20px;
            border: 1px solid var(--rose-border);
        }
        .sheet-title {
            text-align: center;
            font-size: 1.25rem;
            font-weight: 700;
            color: var(--rose-ink);
            margin-bottom: 4px;
            letter-spacing: 0.5px;
        }
        .sheet-subtitle {
            text-align: center;
            font-size: 0.88rem;
            color: #333333;
            margin-bottom: 16px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header no-print">
        <h2 class="page-title">Stat de Plata</h2>
    </div>

    <div class="report-toolbar no-print">
        <div style="display: flex; gap: 10px; align-items: center; flex-wrap: wrap;">
            <label style="font-size: 0.85rem; font-weight: 600;">Luna calcul:</label>
            <asp:TextBox ID="txtLunaCalcul" runat="server" CssClass="form-control" style="width: 140px;" />

            <label style="font-size: 0.85rem; font-weight: 600;">Observatii / Info:</label>
            <asp:TextBox ID="txtInfoManual" runat="server" CssClass="form-control" style="width: 180px;" placeholder="Ex: Sediul Central" />

            <asp:Button ID="btnAplicaAntet" runat="server" Text="Aplica antet" CssClass="btn btn-outline" OnClick="btnAplicaAntet_Click" />
        </div>
        <div style="display: flex; gap: 10px;">
            <button type="button" class="btn btn-primary" onclick="window.print();">
                Tipareste (A3 / PDF)
            </button>
            <asp:Button ID="btnDescarcaCsv" runat="server" Text="Export CSV" 
                CssClass="btn btn-outline" OnClick="btnDescarcaCsv_Click" />
        </div>
    </div>

    <div class="stat-plata-sheet">
        <div class="sheet-title">STAT DE PLATA - <asp:Label ID="lblLunaAfisata" runat="server" /></div>
        <div class="sheet-subtitle">
            DATA: <strong><asp:Label ID="lblDataEmiterii" runat="server" /></strong>
            <asp:Label ID="lblInfoManualAfisat" runat="server" style="margin-left: 15px; font-style: italic;" />
        </div>

        <table class="stat-plata-table">
            <thead>
                <tr>
                    <th style="width: 40px;">NR. CRT</th>
                    <th>NUME</th>
                    <th>PRENUME</th>
                    <th>FUNCTIE</th>
                    <th>SALAR BAZA</th>
                    <th>SPOR %</th>
                    <th>PREMII</th>
                    <th>TOTAL BRUT</th>
                    <th>CAS (<asp:Label ID="lblCasProcent" runat="server" />%)</th>
                    <th>CASS (<asp:Label ID="lblCassProcent" runat="server" />%)</th>
                    <th>BRUT IMPOZABIL</th>
                    <th>IMPOZIT (<asp:Label ID="lblImpozitProcent" runat="server" />%)</th>
                    <th>RETINERI</th>
                    <th>VIRAT CARD</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptStatPlata" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="text-align: center;"><%# Eval("NR_CRT") %></td>
                            <td><%# Eval("NUME") %></td>
                            <td><%# Eval("PRENUME") %></td>
                            <td><%# Eval("FUNCTIE") %></td>
                            <td style="text-align: right;"><%# Eval("SALAR_BAZA", "{0:N0}") %></td>
                            <td style="text-align: right;"><%# Eval("SPOR_PROCENT") %>%</td>
                            <td style="text-align: right;"><%# Eval("PREMII_BRUTE", "{0:N0}") %></td>
                            <td style="text-align: right; font-weight: bold; background-color: #fefce8;"><%# Eval("TOTAL_BRUT", "{0:N0}") %></td>
                            <td style="text-align: right;"><%# Eval("CAS", "{0:N0}") %></td>
                            <td style="text-align: right;"><%# Eval("CASS", "{0:N0}") %></td>
                            <td style="text-align: right;"><%# Eval("BRUT_IMPOZABIL", "{0:N0}") %></td>
                            <td style="text-align: right;"><%# Eval("IMPOZIT", "{0:N0}") %></td>
                            <td style="text-align: right;"><%# Eval("RETINERI", "{0:N0}") %></td>
                            <td style="text-align: right; font-weight: bold; background-color: #f0fdf4; color: #15803d;"><%# Eval("VIRAT_CARD", "{0:N0}") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
            <tfoot>
                <tr class="stat-plata-footer">
                    <td colspan="7" style="text-align: right; font-weight: bold;">TOTAL:</td>
                    <td style="text-align: right; font-weight: bold; font-size: 1.05rem;">
                        <asp:Label ID="lblTotalBrutGeneral" runat="server" />
                    </td>
                    <td colspan="5"></td>
                    <td style="text-align: right; font-weight: bold; font-size: 1.05rem; color: #166534;">
                        <asp:Label ID="lblTotalViratGeneral" runat="server" />
                    </td>
                </tr>
            </tfoot>
        </table>

        <div style="margin-top: 40px; display: flex; justify-content: space-between; font-size: 0.95rem;">
            <div>
                <strong>Intocmit,</strong>
            </div>
            <div>
                <strong>Verificat,</strong>
            </div>
            <div>
                <strong>Aprobat,</strong>
            </div>
        </div>
    </div>
</asp:Content>
