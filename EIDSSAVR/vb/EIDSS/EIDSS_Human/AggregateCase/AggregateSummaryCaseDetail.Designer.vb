<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AggregateSummaryCaseDetail
    Inherits bv.common.win.BaseDetailForm

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
        AggregateCaseDbService = New AggregateSummaryCase_DB

        DbService = AggregateCaseDbService

        PermissionObject = EIDSS.model.Enums.EIDSSPermissionObject.AccessToHumanAggregateCase
        AggregateSummaryHeader1.CaseType = model.Enums.AggregateCaseType.HumanAggregateCase


        '''AggregateSummaryHeader1.UseParentDataset = True
        '''RegisterChildObject(AggregateSummaryHeader1, RelatedPostOrder.SkipPost)
        'RegisterChildObject(fgAggrCase, RelatedPostOrder.PostLast)

        'minYear = 1900
        MenuItem1.Visible = EIDSS.model.Reports.BaseMenuReportRegistrator.IsPaperFormAllowed("HumAggregateCaseSummary")
    End Sub

    'Public Sub New(ByVal _MinYear As Integer)
    '    MyBase.New()

    '    'This call is required by the Windows Form Designer.
    '    InitializeComponent()
    '    'Add any initialization after the InitializeComponent() call
    '    AggregateCaseDbService = New AggregateSummaryCase_DB

    '    DbService = AggregateCaseDbService

    '    PermissionObject = eidss.model.Enums.EIDSSPermissionObject.HumanCase

    '    RegisterChildObject(fgAggrCase, RelatedPostOrder.PostLast)

    '    If (_MinYear) > 0 AndAlso (_MinYear <= Date.Today.Year) Then
    '        minYear = _MinYear
    '    Else
    '        minYear = 1900
    '    End If
    'End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AggregateSummaryCaseDetail))
        Me.gbDiseaseList = New DevExpress.XtraEditors.GroupControl()
        Me.fgAggrCase = New EIDSS.FlexibleForms.FFPresenter()
        Me.lbNoHumanMatrix = New DevExpress.XtraEditors.LabelControl()
        Me.btnShowSummary = New DevExpress.XtraEditors.SimpleButton()
        Me.btnViewDetailForm = New DevExpress.XtraEditors.SimpleButton()
        Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
        Me.btnRefresh = New DevExpress.XtraEditors.SimpleButton()
        Me.PopUpButton1 = New bv.winclient.Core.PopUpButton()
        Me.cmRep = New System.Windows.Forms.ContextMenu()
        Me.MenuItem1 = New System.Windows.Forms.MenuItem()
        Me.AggregateSummaryHeader1 = New eidss.winclient.AggregateCase.AggregateSummaryHeader()
        CType(Me.gbDiseaseList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDiseaseList.SuspendLayout()
        Me.SuspendLayout()
        bv.common.Resources.BvResourceManagerChanger.GetResourceManager(GetType(AggregateSummaryCaseDetail), resources)
        'Form Is Localizable: True
        '
        'gbDiseaseList
        '
        resources.ApplyResources(Me.gbDiseaseList, "gbDiseaseList")
        Me.gbDiseaseList.Controls.Add(Me.fgAggrCase)
        Me.gbDiseaseList.Controls.Add(Me.lbNoHumanMatrix)
        Me.gbDiseaseList.Name = "gbDiseaseList"
        '
        'fgAggrCase
        '
        resources.ApplyResources(Me.fgAggrCase, "fgAggrCase")
        Me.fgAggrCase.DelayedLoadingNeeded = False
        Me.fgAggrCase.DynamicParameterEnabled = False
        Me.fgAggrCase.FormID = Nothing
        Me.fgAggrCase.HelpTopicID = Nothing
        Me.fgAggrCase.KeyFieldName = Nothing
        Me.fgAggrCase.MultiSelect = False
        Me.fgAggrCase.Name = "fgAggrCase"
        Me.fgAggrCase.ObjectName = Nothing
        Me.fgAggrCase.SectionTableCaptionsIsVisible = True
        Me.fgAggrCase.TableName = Nothing
        '
        'lbNoHumanMatrix
        '
        Me.lbNoHumanMatrix.Appearance.ForeColor = CType(resources.GetObject("lbNoHumanMatrix.Appearance.ForeColor"), System.Drawing.Color)
        Me.lbNoHumanMatrix.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lbNoHumanMatrix.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        resources.ApplyResources(Me.lbNoHumanMatrix, "lbNoHumanMatrix")
        Me.lbNoHumanMatrix.Name = "lbNoHumanMatrix"
        '
        'btnShowSummary
        '
        resources.ApplyResources(Me.btnShowSummary, "btnShowSummary")
        Me.btnShowSummary.Name = "btnShowSummary"
        '
        'btnViewDetailForm
        '
        resources.ApplyResources(Me.btnViewDetailForm, "btnViewDetailForm")
        Me.btnViewDetailForm.Image = Global.eidss.My.Resources.Resources.View1
        Me.btnViewDetailForm.Name = "btnViewDetailForm"
        '
        'btnClose
        '
        resources.ApplyResources(Me.btnClose, "btnClose")
        Me.btnClose.Image = Global.eidss.My.Resources.Resources.Close
        Me.btnClose.Name = "btnClose"
        '
        'btnRefresh
        '
        resources.ApplyResources(Me.btnRefresh, "btnRefresh")
        Me.btnRefresh.Image = Global.eidss.My.Resources.Resources.refresh
        Me.btnRefresh.Name = "btnRefresh"
        '
        'PopUpButton1
        '
        resources.ApplyResources(Me.PopUpButton1, "PopUpButton1")
        Me.PopUpButton1.ButtonType = bv.winclient.Core.PopUpButton.PopUpButtonStyles.Reports
        Me.PopUpButton1.Name = "PopUpButton1"
        Me.PopUpButton1.PopUpMenu = Me.cmRep
        Me.PopUpButton1.Tag = "{Immovable}{AlwaysEditable}"
        '
        'cmRep
        '
        Me.cmRep.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        resources.ApplyResources(Me.MenuItem1, "MenuItem1")
        '
        'AggregateSummaryHeader1
        '
        resources.ApplyResources(Me.AggregateSummaryHeader1, "AggregateSummaryHeader1")
        Me.AggregateSummaryHeader1.CaseType = eidss.model.Enums.AggregateCaseType.None
        Me.AggregateSummaryHeader1.Name = "AggregateSummaryHeader1"
        '
        'AggregateSummaryCaseDetail
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.AggregateSummaryHeader1)
        Me.Controls.Add(Me.PopUpButton1)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnViewDetailForm)
        Me.Controls.Add(Me.btnShowSummary)
        Me.Controls.Add(Me.gbDiseaseList)
        Me.DefaultFormState = System.Windows.Forms.FormWindowState.Normal
        Me.FormID = "H08"
        Me.HelpTopicID = "HAC_summary"
        Me.KeyFieldName = "idfAggrCase"
        Me.LeftIcon = Global.eidss.My.Resources.Resources.Human_Aggregate_Cases_Summary__large__10_
        Me.Name = "AggregateSummaryCaseDetail"
        Me.ObjectName = "AggregateSummaryCase"
        Me.ShowCancelButton = False
        Me.ShowDeleteButton = False
        Me.ShowOkButton = False
        Me.ShowSaveButton = False
        Me.Sizable = True
        Me.Status = bv.common.win.FormStatus.Draft
        Me.TableName = "AggregateHeader"
        Me.Controls.SetChildIndex(Me.gbDiseaseList, 0)
        Me.Controls.SetChildIndex(Me.btnShowSummary, 0)
        Me.Controls.SetChildIndex(Me.btnViewDetailForm, 0)
        Me.Controls.SetChildIndex(Me.btnClose, 0)
        Me.Controls.SetChildIndex(Me.btnRefresh, 0)
        Me.Controls.SetChildIndex(Me.PopUpButton1, 0)
        Me.Controls.SetChildIndex(Me.AggregateSummaryHeader1, 0)
        CType(Me.gbDiseaseList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDiseaseList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbDiseaseList As DevExpress.XtraEditors.GroupControl
    Friend WithEvents fgAggrCase As EIDSS.FlexibleForms.FFPresenter
    Friend WithEvents btnShowSummary As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnViewDetailForm As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PopUpButton1 As bv.winclient.Core.PopUpButton
    Friend WithEvents cmRep As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents AggregateSummaryHeader1 As eidss.winclient.AggregateCase.AggregateSummaryHeader
    Friend WithEvents lbNoHumanMatrix As DevExpress.XtraEditors.LabelControl
End Class
