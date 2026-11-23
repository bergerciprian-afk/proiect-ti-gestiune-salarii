<%@ Page Title="Actualizare Date" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActualizareDate.aspx.cs" Inherits="SalariiApp.ActualizareDate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .search-container {
            display: flex;
            gap: 10px;
            align-items: center;
            background: #ffffff;
            padding: 10px 14px;
            border: 1px solid var(--rose-border);
            margin-bottom: 15px;
            flex-wrap: wrap;
        }
        .edit-single-panel {
            background: var(--rose-bg);
            border: 1px solid var(--rose-border);
            padding: 16px;
            margin-bottom: 18px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2 class="page-title">Actualizare Date Angajati</h2>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false">
        <div id="divAlert" runat="server">
            <asp:Label ID="lblAlertMsg" runat="server" />
        </div>
    </asp:Panel>

    <div class="search-container">
        <label style="font-weight: 600; color: #1e293b;">Cautare angajat (Nume / Prenume / Functie):</label>
        <asp:TextBox ID="txtFiltru" runat="server" CssClass="form-control" style="max-width: 300px;" placeholder="Cautare..." />
        <asp:Button ID="btnCauta" runat="server" Text="Cauta" CssClass="btn btn-primary" OnClick="btnCauta_Click" />
        <asp:Button ID="btnReseteazaFiltru" runat="server" Text="Arata toti" CssClass="btn btn-outline" OnClick="btnReseteazaFiltru_Click" />
    </div>

    <asp:Panel ID="pnlEditSingle" runat="server" Visible="false" CssClass="edit-single-panel">
        <h3 style="color: #166534; margin-bottom: 12px;">Editare angajat: <asp:Label ID="lblNumeAngajatSelectat" runat="server" /></h3>
        <asp:HiddenField ID="hfSelectedId" runat="server" />

        <div class="form-grid">
            <div class="form-group">
                <label>Nume:</label>
                <asp:TextBox ID="txtEditNume" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label>Prenume:</label>
                <asp:TextBox ID="txtEditPrenume" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label>Functie:</label>
                <asp:TextBox ID="txtEditFunctie" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label>Salar de baza (RON):</label>
                <asp:TextBox ID="txtEditSalar" runat="server" CssClass="form-control" TextMode="Number" />
            </div>
            <div class="form-group">
                <label>Spor (%):</label>
                <asp:TextBox ID="txtEditSpor" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label>Premii brute (RON):</label>
                <asp:TextBox ID="txtEditPremii" runat="server" CssClass="form-control" TextMode="Number" />
            </div>
            <div class="form-group">
                <label>Retineri (RON):</label>
                <asp:TextBox ID="txtEditRetineri" runat="server" CssClass="form-control" TextMode="Number" />
            </div>
            <div class="form-group">
                <label>Total brut calculat:</label>
                <asp:TextBox ID="txtEditTotalBrut" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>
            <div class="form-group">
                <label>CAS calculat:</label>
                <asp:TextBox ID="txtEditCas" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>
            <div class="form-group">
                <label>CASS calculat:</label>
                <asp:TextBox ID="txtEditCass" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>
            <div class="form-group">
                <label>Brut impozabil calculat:</label>
                <asp:TextBox ID="txtEditBrutImp" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>
            <div class="form-group">
                <label>Impozit calculat:</label>
                <asp:TextBox ID="txtEditImpozit" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>
            <div class="form-group">
                <label>Virat card calculat:</label>
                <asp:TextBox ID="txtEditViratCard" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>
        </div>

        <div style="display: flex; gap: 10px; margin-top: 10px;">
            <asp:Button ID="btnSalveazaModificari" runat="server" Text="Salveaza modificari" CssClass="btn btn-success" OnClick="btnSalveazaModificari_Click" />
            <asp:Button ID="btnAnuleazaEditare" runat="server" Text="Inchide" CssClass="btn btn-outline" OnClick="btnAnuleazaEditare_Click" />
        </div>
    </asp:Panel>

    <div class="card">
        <h3 style="color: var(--rose-ink); margin-bottom: 12px;">Tabel angajati</h3>

        <div style="overflow-x: auto;">
            <asp:GridView ID="gvAngajati" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="NR_CRT"
                CssClass="custom-grid" 
                OnRowEditing="gvAngajati_RowEditing"
                OnRowCancelingEdit="gvAngajati_RowCancelingEdit"
                OnRowUpdating="gvAngajati_RowUpdating"
                OnSelectedIndexChanged="gvAngajati_SelectedIndexChanged"
                EmptyDataText="Nu exista inregistrari.">
                <Columns>
                    <asp:BoundField DataField="NR_CRT" HeaderText="ID" ReadOnly="True" />
                    
                    <asp:TemplateField HeaderText="Nume">
                        <ItemTemplate><%# Eval("NUME") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGvNume" runat="server" Text='<%# Bind("NUME") %>' CssClass="form-control" style="width: 100px;" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Prenume">
                        <ItemTemplate><%# Eval("PRENUME") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGvPrenume" runat="server" Text='<%# Bind("PRENUME") %>' CssClass="form-control" style="width: 100px;" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Functie">
                        <ItemTemplate><%# Eval("FUNCTIE") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGvFunctie" runat="server" Text='<%# Bind("FUNCTIE") %>' CssClass="form-control" style="width: 120px;" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Salar Baza">
                        <ItemTemplate><%# Eval("SALAR_BAZA", "{0:N0}") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGvSalar" runat="server" Text='<%# Bind("SALAR_BAZA") %>' CssClass="form-control" style="width: 80px;" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Spor (%)">
                        <ItemTemplate><%# Eval("SPOR_PROCENT") %>%</ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGvSpor" runat="server" Text='<%# Bind("SPOR_PROCENT") %>' CssClass="form-control" style="width: 60px;" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Premii">
                        <ItemTemplate><%# Eval("PREMII_BRUTE", "{0:N0}") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGvPremii" runat="server" Text='<%# Bind("PREMII_BRUTE") %>' CssClass="form-control" style="width: 70px;" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Retineri">
                        <ItemTemplate><%# Eval("RETINERI", "{0:N0}") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGvRetineri" runat="server" Text='<%# Bind("RETINERI") %>' CssClass="form-control" style="width: 70px;" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="TOTAL_BRUT" HeaderText="Total Brut" ReadOnly="True" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="CAS" HeaderText="CAS" ReadOnly="True" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="CASS" HeaderText="CASS" ReadOnly="True" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="BRUT_IMPOZABIL" HeaderText="Brut Imp." ReadOnly="True" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="IMPOZIT" HeaderText="Impozit" ReadOnly="True" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />
                    <asp:BoundField DataField="VIRAT_CARD" HeaderText="Virat Card" ReadOnly="True" DataFormatString="{0:N0}" ItemStyle-CssClass="col-calc" />

                    <asp:CommandField ShowEditButton="True" HeaderText="Editare tabelara" 
                        EditText="Edit" UpdateText="Salveaza" CancelText="Anuleaza" />

                    <asp:CommandField ShowSelectButton="True" SelectText="Selecteaza" HeaderText="Formular" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
