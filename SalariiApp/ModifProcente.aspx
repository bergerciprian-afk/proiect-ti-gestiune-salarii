<%@ Page Title="Modificare Procente" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModifProcente.aspx.cs" Inherits="SalariiApp.ModifProcente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .auth-card {
            max-width: 440px;
            margin: 30px auto;
            background: #ffffff;
            padding: 22px;
            border: 1px solid var(--rose-border);
        }
        .tax-edit-card {
            max-width: 580px;
            margin: 15px auto;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2 class="page-title">Modificare Procente</h2>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false">
        <div id="divAlert" runat="server">
            <asp:Label ID="lblAlertMsg" runat="server" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlAuth" runat="server" CssClass="auth-card" DefaultButton="btnAutentifica">
        <div style="text-align: center; margin-bottom: 20px;">
            <h3 style="color: #0f172a; margin-bottom: 6px;">Autentificare</h3>
            <p style="font-size: 0.88rem; color: #64748b;">
                Introduceti parola pentru modificarea procentelor.
            </p>
        </div>

        <div class="form-group" style="margin-bottom: 18px;">
            <label for="<%= txtParolaAuth.ClientID %>">Parola:</label>
            <asp:TextBox ID="txtParolaAuth" runat="server" TextMode="Password" CssClass="form-control" />
        </div>

        <asp:Button ID="btnAutentifica" runat="server" Text="Autentificare" CssClass="btn btn-primary" style="width: 100%;" OnClick="btnAutentifica_Click" />
    </asp:Panel>

    <asp:Panel ID="pnlModificare" runat="server" Visible="false" CssClass="card tax-edit-card">
        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; border-bottom: 1px solid #e2e8f0; padding-bottom: 10px;">
            <h3 style="color: var(--rose-ink);">Procente taxe</h3>
            <asp:Button ID="btnBlocheaza" runat="server" Text="Deconectare" CssClass="btn btn-outline" Font-Size="Smaller" OnClick="btnBlocheaza_Click" />
        </div>

        <div class="form-grid" style="grid-template-columns: 1fr;">
            <div class="form-group">
                <label for="<%= txtCas.ClientID %>">CAS - Pensie (%):</label>
                <asp:TextBox ID="txtCas" runat="server" CssClass="form-control" />
                <asp:RangeValidator ID="rvCas" runat="server" ControlToValidate="txtCas" 
                    Type="Double" MinimumValue="0" MaximumValue="100" ErrorMessage="Valoare intre 0 si 100" 
                    ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
            </div>

            <div class="form-group">
                <label for="<%= txtCass.ClientID %>">CASS - Sanatate (%):</label>
                <asp:TextBox ID="txtCass" runat="server" CssClass="form-control" />
                <asp:RangeValidator ID="rvCass" runat="server" ControlToValidate="txtCass" 
                    Type="Double" MinimumValue="0" MaximumValue="100" ErrorMessage="Valoare intre 0 si 100" 
                    ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
            </div>

            <div class="form-group">
                <label for="<%= txtImpozit.ClientID %>">Impozit (%):</label>
                <asp:TextBox ID="txtImpozit" runat="server" CssClass="form-control" />
                <asp:RangeValidator ID="rvImp" runat="server" ControlToValidate="txtImpozit" 
                    Type="Double" MinimumValue="0" MaximumValue="100" ErrorMessage="Valoare intre 0 si 100" 
                    ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
            </div>
        </div>

        <div style="display: flex; gap: 12px; margin-top: 20px;">
            <asp:Button ID="btnSalveazaTaxe" runat="server" Text="Salveaza si recalculeaza" 
                CssClass="btn btn-success" OnClick="btnSalveazaTaxe_Click" />
            <asp:Button ID="btnReseteazaImplicite" runat="server" Text="Valori implicite (25 / 10 / 10)" 
                CssClass="btn btn-outline" CausesValidation="false" OnClick="btnReseteazaImplicite_Click" />
        </div>

        <div style="margin-top: 25px; padding-top: 15px; border-top: 1px solid #e2e8f0;">
            <h4 style="color: var(--rose-ink); margin-bottom: 12px;">Modificare parola acces</h4>
            <div class="form-grid" style="grid-template-columns: 1fr 1fr;">
                <div class="form-group">
                    <label for="<%= txtNouaParola.ClientID %>">Parola noua:</label>
                    <asp:TextBox ID="txtNouaParola" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="<%= txtConfirmaParola.ClientID %>">Confirmare parola noua:</label>
                    <asp:TextBox ID="txtConfirmaParola" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
            </div>
            <div style="margin-top: 10px;">
                <asp:Button ID="btnSchimbaParola" runat="server" Text="Schimba parola" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnSchimbaParola_Click" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
