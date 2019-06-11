Imports bv.winclient.Core
Imports System.Data
Imports EIDSS.model.Resources
Imports System.Collections.Generic
Imports System.Linq
Imports bv.winclient.BasePanel
Imports bv.common.Enums
Imports bv.common.Resources


Public Class GroupCheckInPanel
    Inherits CheckInPanel

    Friend WithEvents MainGridView As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colCaseID As DevExpress.XtraGrid.Columns.GridColumn
    Protected WithEvents btnAddSample As DevExpress.XtraEditors.SimpleButton

    Public Sub New()
        MyBase.New()

        InitializeComponent()
        DbService = New SamplesGroupCheckIn_DB

        colCaseID.VisibleIndex = 1
        colLocation.VisibleIndex = 10
        colCaseID.OptionsColumn.AllowEdit = True
        SamplesGridView.Columns.Add(colCaseID)
        SamplesGridView.Columns.Remove(colSpecies)
        SamplesGridView.Columns.Remove(colAnimal)
        SamplesGridView.Columns.Remove(colVectorID)
        SamplesGridView.Columns.Remove(colVectorType)

        SamplesGroup.Height = Height - SamplesGroup.Top - 8
        SamplesGrid.Height = SamplesGroup.ClientSize.Height - SamplesGrid.Top - 4
        txtSamplesSearchAdvanced.Left = btnAddSample.Left - txtSamplesSearchAdvanced.Width - 8
        lblSamplesSearch.Width = txtSamplesSearchAdvanced.Left - lblSamplesSearch.Left
        pnSpecimenDetail.Visible = False
        NotesGroup.Visible = False
        chkFilterSamples.Visible = False
        ShowNewButton = True
        btnNewSpecimen.Visible = False
        btnSampleSearch.Visible = False
        btnSelectTest.Visible = False

        colTestsCount.Visible = False
        colCaseID.Visible = True
        colSpecimenID.Caption = EidssMessages.Get("colSampleID", "Local/Field Sample ID")
        colSampleCondition.Caption = EidssFields.Get("strComments")
        HACode = eidss.HACode.All
    End Sub


    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GroupCheckInPanel))
        Me.colCaseID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.btnAddSample = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.chkFilterSamples.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSamplesSearchAdvanced.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.memoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbCondition, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbAccessionDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbAccessionDate.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grpSendToOffice, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSendToOffice.SuspendLayout()
        CType(Me.cbSendToOffice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbCollectedByInstitution, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbCollectedByOfficer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbSentToOrganization, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CollectedByGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CollectedByGroup.SuspendLayout()
        CType(Me.cbCollectedByOrganization.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbCollectedByPerson.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbAnimalLookup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbSpecimenType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnSpecimenDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnSpecimenDetail.SuspendLayout()
        CType(Me.NotesGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SamplesGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SamplesGroup.SuspendLayout()
        CType(Me.cbSpecies, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbVectorID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbVectorType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtCollectionDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtCollectionDate.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnAccession
        '
        resources.ApplyResources(Me.btnAccession, "btnAccession")
        '
        'chkFilterSamples
        '
        Me.chkFilterSamples.Properties.Appearance.Options.UseTextOptions = True
        Me.chkFilterSamples.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.chkFilterSamples.Properties.AppearanceDisabled.Options.UseTextOptions = True
        Me.chkFilterSamples.Properties.AppearanceDisabled.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.chkFilterSamples.Properties.AppearanceFocused.Options.UseTextOptions = True
        Me.chkFilterSamples.Properties.AppearanceFocused.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.chkFilterSamples.Properties.AppearanceReadOnly.Options.UseTextOptions = True
        Me.chkFilterSamples.Properties.AppearanceReadOnly.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        '
        'lblSamplesSearch
        '
        Me.lblSamplesSearch.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.lblSamplesSearch.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        resources.ApplyResources(Me.lblSamplesSearch, "lblSamplesSearch")
        '
        'txtSamplesSearchAdvanced
        '
        resources.ApplyResources(Me.txtSamplesSearchAdvanced, "txtSamplesSearchAdvanced")
        '
        'memoEdit
        '
        Me.memoEdit.Appearance.Options.UseFont = True
        Me.memoEdit.AppearanceDisabled.Options.UseFont = True
        Me.memoEdit.AppearanceFocused.Options.UseFont = True
        Me.memoEdit.AppearanceReadOnly.Options.UseFont = True
        '
        'cbCondition
        '
        Me.cbCondition.Appearance.Options.UseFont = True
        Me.cbCondition.AppearanceDisabled.Options.UseFont = True
        Me.cbCondition.AppearanceDropDown.Options.UseFont = True
        Me.cbCondition.AppearanceDropDownHeader.Options.UseFont = True
        Me.cbCondition.AppearanceFocused.Options.UseFont = True
        Me.cbCondition.AppearanceReadOnly.Options.UseFont = True
        '
        'cbAccessionDate
        '
        Me.cbAccessionDate.Appearance.Options.UseFont = True
        Me.cbAccessionDate.AppearanceDisabled.Options.UseFont = True
        Me.cbAccessionDate.AppearanceDropDown.Options.UseFont = True
        Me.cbAccessionDate.AppearanceDropDownHeader.Options.UseFont = True
        Me.cbAccessionDate.AppearanceDropDownHeaderHighlight.Options.UseFont = True
        Me.cbAccessionDate.AppearanceDropDownHighlight.Options.UseFont = True
        Me.cbAccessionDate.AppearanceFocused.Options.UseFont = True
        Me.cbAccessionDate.AppearanceReadOnly.Options.UseFont = True
        Me.cbAccessionDate.AppearanceWeekNumber.Options.UseFont = True
        Me.cbAccessionDate.CalendarTimeProperties.Appearance.Options.UseFont = True
        Me.cbAccessionDate.CalendarTimeProperties.AppearanceDisabled.Options.UseFont = True
        Me.cbAccessionDate.CalendarTimeProperties.AppearanceFocused.Options.UseFont = True
        Me.cbAccessionDate.CalendarTimeProperties.AppearanceReadOnly.Options.UseFont = True
        Me.cbAccessionDate.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton()})
        '
        'grpSendToOffice
        '
        Me.grpSendToOffice.Appearance.Options.UseFont = True
        Me.grpSendToOffice.AppearanceCaption.Options.UseFont = True
        '
        'cbSendToOffice
        '
        Me.cbSendToOffice.Properties.Appearance.Options.UseFont = True
        Me.cbSendToOffice.Properties.AppearanceDisabled.Options.UseFont = True
        Me.cbSendToOffice.Properties.AppearanceDropDown.Options.UseFont = True
        Me.cbSendToOffice.Properties.AppearanceDropDownHeader.Options.UseFont = True
        Me.cbSendToOffice.Properties.AppearanceFocused.Options.UseFont = True
        Me.cbSendToOffice.Properties.AppearanceReadOnly.Options.UseFont = True
        '
        'CollectedByGroup
        '
        Me.CollectedByGroup.Appearance.Options.UseFont = True
        Me.CollectedByGroup.AppearanceCaption.Options.UseFont = True
        '
        'cbCollectedByOrganization
        '
        Me.cbCollectedByOrganization.Properties.Appearance.Options.UseFont = True
        Me.cbCollectedByOrganization.Properties.AppearanceDisabled.Options.UseFont = True
        Me.cbCollectedByOrganization.Properties.AppearanceDropDown.Options.UseFont = True
        Me.cbCollectedByOrganization.Properties.AppearanceDropDownHeader.Options.UseFont = True
        Me.cbCollectedByOrganization.Properties.AppearanceFocused.Options.UseFont = True
        Me.cbCollectedByOrganization.Properties.AppearanceReadOnly.Options.UseFont = True
        resources.ApplyResources(Me.cbCollectedByOrganization, "cbCollectedByOrganization")
        '
        'cbCollectedByPerson
        '
        Me.cbCollectedByPerson.Properties.Appearance.Options.UseFont = True
        Me.cbCollectedByPerson.Properties.AppearanceDisabled.Options.UseFont = True
        Me.cbCollectedByPerson.Properties.AppearanceDropDown.Options.UseFont = True
        Me.cbCollectedByPerson.Properties.AppearanceDropDownHeader.Options.UseFont = True
        Me.cbCollectedByPerson.Properties.AppearanceFocused.Options.UseFont = True
        Me.cbCollectedByPerson.Properties.AppearanceReadOnly.Options.UseFont = True
        resources.ApplyResources(Me.cbCollectedByPerson, "cbCollectedByPerson")
        '
        'btnNewSpecimen
        '
        Me.btnNewSpecimen.Appearance.Options.UseFont = True
        resources.ApplyResources(Me.btnNewSpecimen, "btnNewSpecimen")
        '
        'btnDeleteSpecimen
        '
        Me.btnDeleteSpecimen.Appearance.Options.UseFont = True
        resources.ApplyResources(Me.btnDeleteSpecimen, "btnDeleteSpecimen")
        '
        'cbAnimalLookup
        '
        Me.cbAnimalLookup.Appearance.Options.UseFont = True
        Me.cbAnimalLookup.AppearanceDisabled.Options.UseFont = True
        Me.cbAnimalLookup.AppearanceDropDown.Options.UseFont = True
        Me.cbAnimalLookup.AppearanceDropDownHeader.Options.UseFont = True
        Me.cbAnimalLookup.AppearanceFocused.Options.UseFont = True
        Me.cbAnimalLookup.AppearanceReadOnly.Options.UseFont = True
        '
        'cbSpecimenType
        '
        Me.cbSpecimenType.Appearance.Options.UseFont = True
        Me.cbSpecimenType.AppearanceDisabled.Options.UseFont = True
        Me.cbSpecimenType.AppearanceDropDown.Options.UseFont = True
        Me.cbSpecimenType.AppearanceDropDownHeader.Options.UseFont = True
        Me.cbSpecimenType.AppearanceFocused.Options.UseFont = True
        Me.cbSpecimenType.AppearanceReadOnly.Options.UseFont = True
        '
        'pnSpecimenDetail
        '
        Me.pnSpecimenDetail.Appearance.BackColor = CType(resources.GetObject("pnSpecimenDetail.Appearance.BackColor"), System.Drawing.Color)
        Me.pnSpecimenDetail.Appearance.Options.UseBackColor = True
        Me.pnSpecimenDetail.Appearance.Options.UseFont = True
        Me.pnSpecimenDetail.AppearanceCaption.Options.UseFont = True
        Me.pnSpecimenDetail.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003
        Me.pnSpecimenDetail.LookAndFeel.UseDefaultLookAndFeel = False
        '
        'NotesGroup
        '
        Me.NotesGroup.Appearance.BackColor = CType(resources.GetObject("NotesGroup.Appearance.BackColor"), System.Drawing.Color)
        Me.NotesGroup.Appearance.Options.UseBackColor = True
        Me.NotesGroup.Appearance.Options.UseFont = True
        Me.NotesGroup.AppearanceCaption.Options.UseFont = True
        Me.NotesGroup.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003
        Me.NotesGroup.LookAndFeel.UseDefaultLookAndFeel = False
        '
        'SamplesGroup
        '
        Me.SamplesGroup.Appearance.BackColor = CType(resources.GetObject("SamplesGroup.Appearance.BackColor"), System.Drawing.Color)
        Me.SamplesGroup.Appearance.Options.UseBackColor = True
        Me.SamplesGroup.Appearance.Options.UseFont = True
        Me.SamplesGroup.AppearanceCaption.Options.UseFont = True
        Me.SamplesGroup.Controls.Add(Me.btnAddSample)
        Me.SamplesGroup.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003
        Me.SamplesGroup.LookAndFeel.UseDefaultLookAndFeel = False
        Me.SamplesGroup.Controls.SetChildIndex(Me.btnSampleSearch, 0)
        Me.SamplesGroup.Controls.SetChildIndex(Me.txtSamplesSearchAdvanced, 0)
        Me.SamplesGroup.Controls.SetChildIndex(Me.btnAccession, 0)
        Me.SamplesGroup.Controls.SetChildIndex(Me.chkFilterSamples, 0)
        Me.SamplesGroup.Controls.SetChildIndex(Me.btnNewSpecimen, 0)
        Me.SamplesGroup.Controls.SetChildIndex(Me.btnDeleteSpecimen, 0)
        Me.SamplesGroup.Controls.SetChildIndex(Me.lblSamplesSearch, 0)
        Me.SamplesGroup.Controls.SetChildIndex(Me.btnSelectTest, 0)
        Me.SamplesGroup.Controls.SetChildIndex(Me.btnAddSample, 0)
        '
        'cbSpecies
        '
        Me.cbSpecies.Appearance.Options.UseFont = True
        Me.cbSpecies.AppearanceDisabled.Options.UseFont = True
        Me.cbSpecies.AppearanceDropDown.Options.UseFont = True
        Me.cbSpecies.AppearanceDropDownHeader.Options.UseFont = True
        Me.cbSpecies.AppearanceFocused.Options.UseFont = True
        Me.cbSpecies.AppearanceReadOnly.Options.UseFont = True
        '
        'cbVectorID
        '
        Me.cbVectorID.Appearance.Options.UseFont = True
        Me.cbVectorID.AppearanceDisabled.Options.UseFont = True
        Me.cbVectorID.AppearanceDropDown.Options.UseFont = True
        Me.cbVectorID.AppearanceDropDownHeader.Options.UseFont = True
        Me.cbVectorID.AppearanceFocused.Options.UseFont = True
        Me.cbVectorID.AppearanceReadOnly.Options.UseFont = True
        '
        'cbVectorType
        '
        Me.cbVectorType.Appearance.Options.UseFont = True
        Me.cbVectorType.AppearanceDisabled.Options.UseFont = True
        Me.cbVectorType.AppearanceDropDown.Options.UseFont = True
        Me.cbVectorType.AppearanceDropDownHeader.Options.UseFont = True
        Me.cbVectorType.AppearanceFocused.Options.UseFont = True
        Me.cbVectorType.AppearanceReadOnly.Options.UseFont = True
        '
        'dtCollectionDate
        '
        Me.dtCollectionDate.Appearance.Options.UseFont = True
        Me.dtCollectionDate.AppearanceDisabled.Options.UseFont = True
        Me.dtCollectionDate.AppearanceDropDown.Options.UseFont = True
        Me.dtCollectionDate.AppearanceDropDownHeader.Options.UseFont = True
        Me.dtCollectionDate.AppearanceDropDownHeaderHighlight.Options.UseFont = True
        Me.dtCollectionDate.AppearanceDropDownHighlight.Options.UseFont = True
        Me.dtCollectionDate.AppearanceFocused.Options.UseFont = True
        Me.dtCollectionDate.AppearanceReadOnly.Options.UseFont = True
        Me.dtCollectionDate.AppearanceWeekNumber.Options.UseFont = True
        Me.dtCollectionDate.CalendarTimeProperties.Appearance.Options.UseFont = True
        Me.dtCollectionDate.CalendarTimeProperties.AppearanceDisabled.Options.UseFont = True
        Me.dtCollectionDate.CalendarTimeProperties.AppearanceFocused.Options.UseFont = True
        Me.dtCollectionDate.CalendarTimeProperties.AppearanceReadOnly.Options.UseFont = True
        Me.dtCollectionDate.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton()})
        bv.common.Resources.BvResourceManagerChanger.GetResourceManager(GetType(GroupCheckInPanel), resources)
        'Form Is Localizable: True
        '
        'colCaseID
        '
        resources.ApplyResources(Me.colCaseID, "colCaseID")
        Me.colCaseID.FieldName = "strCase"
        Me.colCaseID.Name = "colCaseID"
        '
        'btnAddSample
        '
        Me.btnAddSample.Image = Global.EIDSS.My.Resources.Resources.add
        resources.ApplyResources(Me.btnAddSample, "btnAddSample")
        Me.btnAddSample.Name = "btnAddSample"
        Me.btnAddSample.Tag = "{Immovable}"
        '
        'GroupCheckInPanel
        '
        Me.Appearance.Options.UseFont = True
        Me.DefaultFormState = System.Windows.Forms.FormWindowState.Normal
        Me.Name = "GroupCheckInPanel"
        Me.Status = bv.common.win.FormStatus.Draft
        CType(Me.chkFilterSamples.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSamplesSearchAdvanced.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.memoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbCondition, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbAccessionDate.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbAccessionDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grpSendToOffice, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSendToOffice.ResumeLayout(False)
        Me.grpSendToOffice.PerformLayout()
        CType(Me.cbSendToOffice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbCollectedByInstitution, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbCollectedByOfficer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbSentToOrganization, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CollectedByGroup, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CollectedByGroup.ResumeLayout(False)
        Me.CollectedByGroup.PerformLayout()
        CType(Me.cbCollectedByOrganization.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbCollectedByPerson.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbAnimalLookup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbSpecimenType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnSpecimenDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnSpecimenDetail.ResumeLayout(False)
        CType(Me.NotesGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SamplesGroup, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SamplesGroup.ResumeLayout(False)
        CType(Me.cbSpecies, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbVectorID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbVectorType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtCollectionDate.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtCollectionDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected Overrides Sub DefineBinding()

        Core.LookupBinder.BindSampleRepositoryLookup(cbSpecimenType, HACode, False)
        Core.LookupBinder.BindBaseRepositoryLookup(cbCondition, BaseReferenceType.rftAccessionCondition, HACode, False)
        LabUtils.BindFreezerLocation(cbLocation)
        Core.LookupBinder.BindDepartmentRepositoryLookup(cbDepartment)
        AddHandler btnAddSample.Click, AddressOf btnAddSample_Click

    End Sub

    Protected Overrides Sub SearchByBarcode(ByVal barCode As String)
        If Utils.IsEmpty(barCode) Then
            Return
        End If
        If LockHandler() Then
            Try
                ' Seek a material by barcode
                Dim foundSamplesDataset As DataSet = DbService.GetDetail(barCode)

                If (foundSamplesDataset Is Nothing) OrElse Not foundSamplesDataset.Tables.Contains(CaseSamples_Db.TableSamples) _
                                        OrElse (foundSamplesDataset.Tables(CaseSamples_Db.TableSamples).Rows.Count <= 0) Then
                    'show message "not found"
                    Dim msg As String = EidssMessages.Get("msgSamplesForGroupAccesionInNotFound")
                    Dim msgCaption As String = EidssMessages.Get("ErrFieldSampleIDNotFound", "Sample not found")
                    MessageForm.Show(msg, msgCaption)
                    Return
                End If
                'Delete rows that are in our dataset already
                If foundSamplesDataset.Tables(CaseSamples_Db.TableSamples).Rows.Count > 1 AndAlso baseDataSet.Tables.Contains(CaseSamples_Db.TableSamples) Then
                    For i As Integer = foundSamplesDataset.Tables(CaseSamples_Db.TableSamples).Rows.Count - 1 To 0 Step -1
                        Dim row As DataRow = foundSamplesDataset.Tables(CaseSamples_Db.TableSamples).Rows(i)
                        If Not baseDataSet.Tables(CaseSamples_Db.TableSamples).Rows.Find(row("idfMaterial")) Is Nothing Then
                            row.Delete()
                            row.AcceptChanges()
                        End If
                    Next
                End If
                ' Check that 2 materials are found 

                Dim selectedSamples As DataRow() = Nothing
                If foundSamplesDataset.Tables(CaseSamples_Db.TableSamples).Rows.Count > 1 Then

                    Dim form As New SampleGroupCheckInShortList
                    Dim dict As New Dictionary(Of String, Object)
                    dict.Add("datasource", foundSamplesDataset.Tables(CaseSamples_Db.TableSamples))
                    Dim id As Object = Nothing
                    If BaseFormManager.ShowModal(CType(form, IApplicationForm), FindForm, id, Nothing, dict, Nothing, Nothing) Then
                        selectedSamples = CType(form, BaseForm).GetSelectedRows()
                    Else
                        Return
                    End If
                ElseIf foundSamplesDataset.Tables(CaseSamples_Db.TableSamples).Rows.Count = 1 Then
                    selectedSamples = {foundSamplesDataset.Tables(CaseSamples_Db.TableSamples).Rows(0)}
                End If
                If selectedSamples Is Nothing Then
                    Return
                End If
                SamplesView.OptionsBehavior.Editable = False 'True
                baseDataSet.Merge(selectedSamples)
                If SamplesGrid.DataSource Is Nothing Then
                    SamplesGrid.DataSource = baseDataSet.Tables(CaseSamples_Db.TableSamples)
                End If
            Catch ex As Exception
                Throw
            Finally
                UnlockHandler()
            End Try
        End If

    End Sub

    Protected Sub btnAddSample_Click(ByVal sender As Object, ByVal e As EventArgs)
        SearchByBarcode(txtSamplesSearchAdvanced.Text)
    End Sub

    Overrides Sub btnDeleteSpecimen_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim sampleRow As DataRow = SamplesGridView.GetDataRow(SamplesGridView.FocusedRowHandle)
        If sampleRow Is Nothing Then Return
        If sampleRow.RowState <> DataRowState.Added AndAlso Utils.Str(sampleRow("Used")) = "1" Then
            ErrorForm.ShowMessage("msgCantDeleteRecord", "The record can't be deleted")
            Return
        End If
        If CaseSamples_Db.CheckAccessIn(CLng(SamplesGridView.GetDataRow(SamplesGridView.FocusedRowHandle)("idfMaterial"))) = False Then
            ErrorForm.ShowMessage("msgCantDeleteRecord", "The record can't be deleted")
            Return
        End If
        If WinUtils.ConfirmDelete = False Then
            Return
        End If
        Dim material As Long = CType(sampleRow("idfMaterial"), Long)

        Dim row As DataRow = (From result In baseDataSet.Tables(CaseSamples_Db.TableSamples).AsEnumerable
            Where result.Field(Of Long)("idfMaterial") = material).First()

        row.Delete()
        baseDataSet.AcceptChanges()

    End Sub

    Overrides Sub ShowSampleDetails(ByVal sampleIndex As Integer)
        SetLookupReadOnly(cbCollectedByOrganization, False)
    End Sub

    Public Overrides Sub UpdateButtons()
        If Enabled = False Then Exit Sub
        Dim row As DataRow = SamplesGridView.GetFocusedDataRow()
        Dim specimenSelected As Boolean = Not row Is Nothing
        btnDeleteSpecimen.Enabled = specimenSelected ' AndAlso row.RowState = DataRowState.Added
        pnSpecimenDetail.Enabled = specimenSelected

        btnAccession.Enabled = Not [ReadOnly] AndAlso specimenSelected AndAlso Not IsAccessioned(row)
        cbCondition.MaxLength = 300
        cbAccessionDate.MaxLength = 300
        BarcodeButton1.Enabled = m_CanPrint AndAlso specimenSelected AndAlso Not Utils.IsEmpty(row("strBarcode"))
    End Sub

    Private Sub AccessionRow(ByRef row As DataRow)
        row("strBarcode") = NumberingService.GetNextNumber(NumberingObject.Sample, DbService.Connection, Nothing)
        row("idfAccesionByPerson") = model.Core.EidssUserContext.User.EmployeeID
        row("datAccession") = DateTime.Today
        row("Used") = 1 ' dummy, just to mark sample as accessioned
        row("idfsAccessionCondition") = 10108001 'AccessionCondition.AcceptedGood
    End Sub

    Private Function IsAccessioned(row As DataRow) As Boolean
        Return Not row Is Nothing AndAlso Utils.Str(row("Used")) = "1"
    End Function
    Overrides Sub btnAccession_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim grid As DevExpress.XtraGrid.Views.Grid.GridView = CType(SamplesGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

        For row As Integer = 0 To grid.RowCount - 1 Step 1
            Dim data As DataRow = SamplesGridView.GetDataRow(row)
            If Not IsAccessioned(data) Then
                AccessionRow(data)
            End If
        Next
        SamplesView.OptionsBehavior.Editable = True
        UpdateButtons()
    End Sub
    Public Overrides Function IsCurrentSpecimenRowValid(Optional ByVal index As Integer = -1, Optional ByVal showError As Boolean = True) As Boolean
        If index = -1 Then index = SamplesGridView.FocusedRowHandle
        If index < 0 Then Return True
        Dim row As DataRow = SamplesGridView.GetDataRow(index)

        If row Is Nothing Then Return True
        If Not IsAccessioned(row) Then
            Return True
        End If
        If (Utils.IsEmpty(row("datAccession"))) Then
            If showError Then
                Dim msg As String = StandardErrorHelper.Error(StandardError.Mandatory, SamplesGridView.Columns("datAccession").Caption)
                ErrorForm.ShowWarningDirect(msg)
            End If
            Return False
        End If
        Dim condition As Object = row("idfsAccessionCondition")
        If Utils.IsEmpty(condition) = False AndAlso (condition.ToString() = AccessionCondition.AcceptedPoor.ToString("d") Or condition.ToString() = AccessionCondition.Rejected.ToString("d")) Then
            If Utils.Str(row("strCondition")).Trim().Length = 0 Then
                SamplesGridView.FocusedColumn = colSampleCondition
                If showError Then
                    Dim msg As String = String.Format(EidssMessages.Get("ErrSampleConditionRequired", "Please enter a comment describing why the {0} is identified as ""{1}"""), colConditionReceived.Caption, SamplesGridView.GetRowCellDisplayText(index, colConditionReceived))
                    ErrorForm.ShowWarningDirect(msg)
                End If
                Return False
            End If
        End If
        If Utils.Str(condition) = AccessionCondition.AcceptedGood.ToString("d") Or Utils.Str(condition) = AccessionCondition.AcceptedPoor.ToString("d") Then
            If Utils.Str(row("strBarcode")).Length = 0 Then
                If showError Then
                    WinUtils.ShowMandatoryError(SamplesGridView.Columns("strBarcode").Caption)
                End If
                Return False
            End If
        End If
        If ShowEditor(row) Then
            If SamplesCheckIn_DB.CheckBarcodeForExist(row) = True Then
                If showError Then
                    Dim msg As String = String.Format(BvMessages.Get("ErrExistingBarcode", "Sample with Lab Sample ID '{0}' already exists in the system. Please enter unique Lab Sample ID value."), Utils.Str(row("strBarcode")))
                    ErrorForm.ShowWarningDirect(msg)
                End If
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub GroupCheckInPanel_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ArrangeButtons(SamplesGroup, btnSelectTest.Top, Nothing)
    End Sub

    Public Overrides Function ValidateData() As Boolean
        If SamplesGridView.FocusedRowHandle >= 0 Then
            If Not IsCurrentSpecimenRowValid(SamplesGridView.FocusedRowHandle, True) Then
                SamplesGrid.Select()
                Return False
            End If
        End If
        Return True
    End Function

End Class


