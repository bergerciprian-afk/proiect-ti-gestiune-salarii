<%@ Page Title="Stergere Angajat" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StergereAngajat.aspx.cs" Inherits="SalariiApp.StergereAngajat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .search-delete-box {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)) auto;
            gap: 12px;
            align-items: flex-end;
            background: #ffffff;
            padding: 12px 16px;
            border: 1px solid var(--rose-border);
            margin-bottom: 15px;
        }
        .confirm-delete-card {
            background-color: #fff8f8;
            border: 1px solid #c62828;
            padding: 14px 18px;
            margin-bottom: 15px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2 class="page-title">Stergere Angajati</h2>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false">
        <div id="divAlert" runat="server">
            <asp:Label ID="lblAlertMsg" runat="server" />
        </div>
    </asp:Panel>

    <div class="search-delete-box">
        <div class="form-group">
            <label>Nume angajat:</label>
            <asp:TextBox ID="txtCautaNume" runat="server" CssClass="form-control" placeholder="Popescu" />
        </div>
        <div class="form-group">
            <label>Prenume angajat:</label>
            <asp:TextBox ID="txtCautaPrenume" runat="server" CssClass="form-control" placeholder="Ion" />
        </div>
        <div>
            <asp:Button ID="btnCauta" runat="server" Text="Cauta" CssClass="btn btn-primary" OnClick="btnCauta_Click" />
            <asp:Button ID="btnArataToti" runat="server" Text="Arata toti" CssClass="btn btn-outline" OnClick="btnArataToti_Click" />
        </div>
    </div>

    <asp:Panel ID="pnlConfirmare" runat="server" Visible="false" CssClass="confirm-delete-card">
        <h3 style="color: #991b1b; margin-bottom: 8px;">Confirmare stergere</h3>
        <p style="margin-bottom: 14px; font-size: 0.95rem;">
            Confirmati stergerea angajatului: 
            <strong><asp:Label ID="lblConfirmNume" runat="server" /></strong> 
            (Functie: <strong><asp:Label ID="lblConfirmFunctie" runat="server" /></strong>, 
            Salar Baza: <strong><asp:Label ID="lblConfirmSalar" runat="server" /> RON</strong>)?
        </p>
        <asp:HiddenField ID="hfDeleteId" runat="server" />
        <div style="display: flex; gap: 10px;">
            <asp:Button ID="btnExecutaStergere" runat="server" Text="Confirma stergere" 
                CssClass="btn btn-danger" OnClick="btnExecutaStergere_Click" 
                OnClientClick="return confirm('Confirmati stergerea?');" />
            <asp:Button ID="btnAnuleazaStergere" runat="server" Text="Anuleaza" 
                CssClass="btn btn-outline" OnClick="btnAnuleazaStergere_Click" />
        </div>
    </asp:Panel>

    <div class="card">
        <h3 style="color: var(--rose-ink); margin-bottom: 12px;">Rezultate cautare</h3>
        
        <div style="overflow-x: auto;">
            <asp:GridView ID="gvStergere" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="NR_CRT"
                CssClass="custom-grid" 
                OnSelectedIndexChanged="gvStergere_SelectedIndexChanged"
                EmptyDataText="Nu exista inregistrari.">
                <Columns>
                    <asp:BoundField DataField="NR_CRT" HeaderText="ID" />
                    <asp:BoundField DataField="NUME" HeaderText="Nume" />
                    <asp:BoundField DataField="PRENUME" HeaderText="Prenume" />
                    <asp:BoundField DataField="FUNCTIE" HeaderText="Functie" />
                    <asp:BoundField DataField="SALAR_BAZA" HeaderText="Salar Baza" DataFormatString="{0:N0} RON" />
                    <asp:BoundField DataField="TOTAL_BRUT" HeaderText="Total Brut" DataFormatString="{0:N0} RON" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="VIRAT_CARD" HeaderText="Virat Card" DataFormatString="{0:N0} RON" ItemStyle-CssClass="col-calc" />
                    
                    <asp:CommandField ShowSelectButton="True" SelectText="Sterge" 
                        ControlStyle-CssClass="btn btn-danger" HeaderText="Actiune" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
