<%@ Page Title="Calcul Salarii" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CalculSalarii.aspx.cs" Inherits="SalariiApp.CalculSalarii" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .calc-header-bar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            gap: 12px;
            background: #ffffff;
            padding: 10px 14px;
            border: 1px solid var(--rose-border);
            margin-bottom: 15px;
        }
        .current-tax-badges {
            display: flex;
            gap: 10px;
            align-items: center;
        }
        .tax-badge {
            background-color: var(--rose-panel);
            color: var(--rose-ink);
            padding: 4px 10px;
            border: 1px solid var(--rose-border);
            font-size: 0.85rem;
            font-weight: 700;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2 class="page-title">Calcul Salarii</h2>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false">
        <div id="divAlert" runat="server">
            <asp:Label ID="lblAlertMsg" runat="server" />
        </div>
    </asp:Panel>

    <div class="calc-header-bar">
        <div class="current-tax-badges">
            <span style="font-size: 0.85rem; font-weight: 700; color: #475569;">Cote aplicate:</span>
            <span class="tax-badge">CAS: <asp:Label ID="lblBadgeCas" runat="server" />%</span>
            <span class="tax-badge">CASS: <asp:Label ID="lblBadgeCass" runat="server" />%</span>
            <span class="tax-badge">Impozit: <asp:Label ID="lblBadgeImp" runat="server" />%</span>
        </div>

        <div style="display: flex; gap: 10px; align-items: center;">
            <asp:TextBox ID="txtFiltru" runat="server" CssClass="form-control" placeholder="Filtrare angajat..." style="width: 200px;" />
            <asp:Button ID="btnFiltreaza" runat="server" Text="Filtreaza" CssClass="btn btn-outline" OnClick="btnFiltreaza_Click" />
            <asp:Button ID="btnRecalculeazaTot" runat="server" Text="Recalculeaza toate salariile" 
                CssClass="btn btn-success" OnClick="btnRecalculeazaTot_Click" 
                OnClientClick="return confirm('Declansati recalcularea salariilor?');" />
        </div>
    </div>

    <div class="card">
        <h3 style="color: var(--rose-ink); margin-bottom: 12px;">Centralizator salarii</h3>

        <div style="overflow-x: auto;">
            <asp:GridView ID="gvCalcul" runat="server" AutoGenerateColumns="False" 
                CssClass="custom-grid" 
                ShowFooter="True"
                EmptyDataText="Nu exista inregistrari.">
                <Columns>
                    <asp:BoundField DataField="NR_CRT" HeaderText="Nr." />
                    <asp:BoundField DataField="NUME" HeaderText="Nume" />
                    <asp:BoundField DataField="PRENUME" HeaderText="Prenume" />
                    <asp:BoundField DataField="FUNCTIE" HeaderText="Functie" />
                    <asp:BoundField DataField="SALAR_BAZA" HeaderText="Salar Baza" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="SPOR_PROCENT" HeaderText="Spor %" DataFormatString="{0}%" />
                    <asp:BoundField DataField="PREMII_BRUTE" HeaderText="Premii" DataFormatString="{0:N0}" />
                    
                    <asp:BoundField DataField="TOTAL_BRUT" HeaderText="Total Brut" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="CAS" HeaderText="CAS" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="CASS" HeaderText="CASS" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="BRUT_IMPOZABIL" HeaderText="Brut Imp." DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="IMPOZIT" HeaderText="Impozit" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="RETINERI" HeaderText="Retineri" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="VIRAT_CARD" HeaderText="Virat Card" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                </Columns>
                <FooterStyle CssClass="custom-grid-footer" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
