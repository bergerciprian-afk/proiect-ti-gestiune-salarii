<%@ Page Title="Adaugare Angajat" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdaugareAngajat.aspx.cs" Inherits="SalariiApp.AdaugareAngajat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .calc-preview-card {
            background-color: #ffffe0;
            border: 1px solid #c2b840;
            padding: 14px 18px;
            margin-top: 15px;
        }
        .calc-preview-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(170px, 1fr));
            gap: 10px;
            margin-top: 8px;
        }
        .calc-item {
            background: #ffffff;
            border: 1px solid #d0c040;
            padding: 8px 12px;
        }
        .calc-item-label { font-size: 0.75rem; color: #555500; font-weight: 700; text-transform: uppercase; }
        .calc-item-value { font-size: 1.15rem; font-weight: 700; color: #222200; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2 class="page-title">Adaugare Angajat</h2>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false">
        <div id="divAlert" runat="server">
            <asp:Label ID="lblAlertMsg" runat="server" />
        </div>
    </asp:Panel>

    <div class="card">
        <h3 style="margin-bottom: 16px; color: var(--rose-ink);">Date angajat</h3>

        <div class="form-grid">
            <div class="form-group">
                <label for="<%= txtNume.ClientID %>">Nume: *</label>
                <asp:TextBox ID="txtNume" runat="server" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator ID="rfvNume" runat="server" ControlToValidate="txtNume" 
                    ErrorMessage="Numele este obligatoriu" ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
            </div>

            <div class="form-group">
                <label for="<%= txtPrenume.ClientID %>">Prenume: *</label>
                <asp:TextBox ID="txtPrenume" runat="server" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator ID="rfvPrenume" runat="server" ControlToValidate="txtPrenume" 
                    ErrorMessage="Prenumele este obligatoriu" ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
            </div>

            <div class="form-group">
                <label for="<%= txtFunctie.ClientID %>">Functie: *</label>
                <asp:TextBox ID="txtFunctie" runat="server" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator ID="rfvFunctie" runat="server" ControlToValidate="txtFunctie" 
                    ErrorMessage="Functia este obligatorie" ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
            </div>

            <div class="form-group">
                <label for="<%= txtSalarBaza.ClientID %>">Salar de baza (RON): *</label>
                <asp:TextBox ID="txtSalarBaza" runat="server" CssClass="form-control" TextMode="Number" />
                <asp:RequiredFieldValidator ID="rfvSalar" runat="server" ControlToValidate="txtSalarBaza" 
                    ErrorMessage="Salariul de baza este obligatoriu" ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
                <asp:RangeValidator ID="rvSalar" runat="server" ControlToValidate="txtSalarBaza"
                    Type="Integer" MinimumValue="1" MaximumValue="2000000" ErrorMessage="Valoare invalida" 
                    ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
            </div>

            <div class="form-group">
                <label for="<%= txtSpor.ClientID %>">Spor (%):</label>
                <asp:TextBox ID="txtSpor" runat="server" CssClass="form-control" Text="0" />
                <asp:RangeValidator ID="rvSpor" runat="server" ControlToValidate="txtSpor"
                    Type="Double" MinimumValue="0" MaximumValue="1000" ErrorMessage="Spor intre 0 si 1000%" 
                    ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
            </div>

            <div class="form-group">
                <label for="<%= txtPremii.ClientID %>">Premii brute (RON):</label>
                <asp:TextBox ID="txtPremii" runat="server" CssClass="form-control" Text="0" TextMode="Number" />
                <asp:RangeValidator ID="rvPremii" runat="server" ControlToValidate="txtPremii"
                    Type="Integer" MinimumValue="0" MaximumValue="1000000" ErrorMessage="Valoare invalida" 
                    ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
            </div>

            <div class="form-group">
                <label for="<%= txtRetineri.ClientID %>">Retineri (RON):</label>
                <asp:TextBox ID="txtRetineri" runat="server" CssClass="form-control" Text="0" TextMode="Number" />
                <asp:RangeValidator ID="rvRetineri" runat="server" ControlToValidate="txtRetineri"
                    Type="Integer" MinimumValue="0" MaximumValue="1000000" ErrorMessage="Valoare invalida" 
                    ForeColor="#dc2626" Display="Dynamic" Font-Size="Smaller" />
            </div>

            <div class="form-group">
                <label for="<%= fuPoza.ClientID %>">Poza angajat (optional BLOB):</label>
                <asp:FileUpload ID="fuPoza" runat="server" CssClass="form-control" accept="image/*" />
            </div>
        </div>

        <div style="display: flex; gap: 12px; margin-top: 10px;">
            <asp:Button ID="btnSalveaza" runat="server" Text="Salveaza angajat" 
                CssClass="btn btn-primary" OnClick="btnSalveaza_Click" />
            <asp:Button ID="btnReseteaza" runat="server" Text="Reseteaza" 
                CssClass="btn btn-outline" CausesValidation="false" OnClick="btnReseteaza_Click" />
        </div>
    </div>

    <asp:Panel ID="pnlRezultatCalcul" runat="server" Visible="false" CssClass="calc-preview-card">
        <h4 style="color: #854d0e;">
            Angajat inregistrat. Rezultate calcul salar:
        </h4>
        <div class="calc-preview-grid">
            <div class="calc-item">
                <div class="calc-item-label">Total Brut</div>
                <div class="calc-item-value"><asp:Label ID="lblResTotalBrut" runat="server" /> RON</div>
            </div>
            <div class="calc-item">
                <div class="calc-item-label">CAS (<asp:Label ID="lblResCasProc" runat="server"/>%)</div>
                <div class="calc-item-value"><asp:Label ID="lblResCas" runat="server" /> RON</div>
            </div>
            <div class="calc-item">
                <div class="calc-item-label">CASS (<asp:Label ID="lblResCassProc" runat="server"/>%)</div>
                <div class="calc-item-value"><asp:Label ID="lblResCass" runat="server" /> RON</div>
            </div>
            <div class="calc-item">
                <div class="calc-item-label">Brut Impozabil</div>
                <div class="calc-item-value"><asp:Label ID="lblResBrutImp" runat="server" /> RON</div>
            </div>
            <div class="calc-item">
                <div class="calc-item-label">Impozit (<asp:Label ID="lblResImpProc" runat="server"/>%)</div>
                <div class="calc-item-value"><asp:Label ID="lblResImpozit" runat="server" /> RON</div>
            </div>
            <div class="calc-item" style="border: 2px solid #16a34a; background: #f0fdf4;">
                <div class="calc-item-label" style="color: #166534;">VIRAT CARD</div>
                <div class="calc-item-value" style="color: #15803d;"><asp:Label ID="lblResViratCard" runat="server" /> RON</div>
            </div>
        </div>
        <p style="font-size: 0.85rem; color: #713f12; margin-top: 10px;">
            Formularul ramane disponibil pentru o noua inregistrare.
        </p>
    </asp:Panel>
</asp:Content>
