Imports System.ComponentModel
Imports bv.winclient.Core
Imports eidss.Core
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports eidss.model.Resources
Imports bv.common.Resources
Imports EIDSS.model.Enums
Imports bv.common.win.Validators

Public Class AggregateHeader
    Private AggregateHeaderDbService As AggregateHeader_DB
    Private m_IsCaseEnabled As Boolean = False
    Private m_CanEdit As Boolean = False
    Private Const MinYear As Integer = 1900
    Private m_Helper As AggregateHelper
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AggregateHeaderDbService = New AggregateHeader_DB
        DbService = AggregateHeaderDbService
        m_Helper = New AggregateHelper(Me, cbYear, cbQuarter, cbMonth, datDay, cbWeek, cbCountry, cbRegion, cbRayon, cbSettlement)

    End Sub

#Region "Private methods"
    Private Function CurrentRow() As DataRow
        Return baseDataSet.Tables(AggregateHeader_DB.TableAggregateHeader).Rows(0)
    End Function
    Private Sub InitStartupParameters()
        Dim parentForm As BaseForm = CType(ParentObject, BaseForm)
        If (Not parentForm.DbService Is Nothing) AndAlso parentForm.DbService.IsNewObject AndAlso _
           (Not parentForm.StartUpParameters Is Nothing) Then
            Me.StartUpParameters = parentForm.StartUpParameters
            If Me.StartUpParameters.ContainsKey("Country") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("Country"))) Then
                CurrentRow("idfsCountry") = Me.StartUpParameters("Country")
                If Me.StartUpParameters.ContainsKey("Region") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("Region"))) Then
                    CurrentRow()("idfsRegion") = Me.StartUpParameters("Region")
                    If Me.StartUpParameters.ContainsKey("Rayon") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("Rayon"))) Then
                        CurrentRow()("idfsRayon") = Me.StartUpParameters("Rayon")
                        If Me.StartUpParameters.ContainsKey("Settlement") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("Settlement"))) Then
                            CurrentRow()("idfsSettlement") = Me.StartUpParameters("Settlement")
                        End If
                    End If
                End If
            End If
            If Me.StartUpParameters.ContainsKey("AdminUnit") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("AdminUnit"))) Then
                CurrentRow()("idfsAdministrativeUnit") = Me.StartUpParameters("AdminUnit")
            End If
            If Me.StartUpParameters.ContainsKey("Year") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("Year"))) Then
                CurrentRow()("YearForAggr") = Me.StartUpParameters("Year")
                If Me.StartUpParameters.ContainsKey("Quarter") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("Quarter"))) Then
                    CurrentRow()("QuarterForAggr") = Me.StartUpParameters("Quarter")
                End If
                If Me.StartUpParameters.ContainsKey("Month") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("Month"))) Then
                    CurrentRow()("MonthForAggr") = Me.StartUpParameters("Month")
                End If
                If Me.StartUpParameters.ContainsKey("Week") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("Week"))) Then
                    CurrentRow()("WeekForAggr") = Me.StartUpParameters("Week")
                End If
                If Me.StartUpParameters.ContainsKey("Day") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("Day"))) Then
                    CurrentRow()("DayForAggr") = Me.StartUpParameters("Day")
                End If
            End If
            If Me.StartUpParameters.ContainsKey("StartDate") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("StartDate"))) Then
                CurrentRow()("datStartDate") = Me.StartUpParameters("StartDate")
                If Me.StartUpParameters.ContainsKey("FinishDate") AndAlso (Not Utils.IsEmpty(Me.StartUpParameters("FinishDate"))) Then
                    CurrentRow()("datFinishDate") = Me.StartUpParameters("FinishDate")
                End If
            End If
        End If

    End Sub

#End Region
#Region "Binding"
    Protected Overrides Sub DefineBinding()
        Dbg.Assert(m_Helper.CaseType <> AggregateCaseType.None, "Aggregate CaseType is not defined")

        InitStartupParameters()
        InitCustomMandatoryFields(m_Helper.CaseType)
        Core.LookupBinder.BindTextEdit(txtCaseID, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".strCaseID")

        Dim haCode As HACode
        If m_Helper.CaseType = AggregateCaseType.AggregateCase Then
            haCode = eidss.HACode.Human
        Else
            haCode = eidss.HACode.Avian Or eidss.HACode.Livestock
        End If
        Core.LookupBinder.BindDateEdit(dtReportingDate, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".datSentByDate")
        Core.LookupBinder.BindOrganizationLookup(cbRepSource, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".idfSentByOffice", haCode)
        Core.LookupBinder.BindPersonLookup(cbRepBy, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".idfSentByPerson", haCode)
        eventManager.AttachDataHandler(AggregateHeader_DB.TableAggregateHeader + ".idfSentByOffice", AddressOf SentByOffice_Changed)
        eventManager.AttachDataHandler(AggregateHeader_DB.TableAggregateHeader + ".idfSentByPerson", AddressOf SentByPerson_Changed)
        Core.LookupBinder.SetPersonFilter(CurrentRow(), "idfSentByOffice", cbRepBy, True)

        Core.LookupBinder.BindDateEdit(dtReceivedDate, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".datReceivedByDate")
        Core.LookupBinder.BindOrganizationLookup(cbReceivedInst, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".idfReceivedByOffice", haCode)
        Core.LookupBinder.BindPersonLookup(cbReceivedBy, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".idfReceivedByPerson", haCode)
        eventManager.AttachDataHandler(AggregateHeader_DB.TableAggregateHeader + ".idfReceivedByOffice", AddressOf ReceivedByOffice_Changed)
        eventManager.AttachDataHandler(AggregateHeader_DB.TableAggregateHeader + ".idfReceivedByPerson", AddressOf ReceivedByPerson_Changed)
        Core.LookupBinder.SetPersonFilter(CurrentRow(), "idfReceivedByOffice", cbReceivedBy, True)

        Core.LookupBinder.BindDateEdit(dtEnteringDate, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".datEnteredByDate")
        Core.LookupBinder.BindOrganizationLookup(cbEnteredInstitution, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".idfEnteredByOffice", HACode.Human)
        Core.LookupBinder.BindPersonLookup(cbEntOfficer, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".idfEnteredByPerson", haCode)
        cbEnteredInstitution.Enabled = False
        cbEntOfficer.Enabled = False
        dtEnteringDate.Enabled = False

        BindTime()
        BindArea()
        m_Helper.BindPeriodFileds()
        If m_Helper.CurrentYear < 0 Then
            m_Helper.CurrentYear = DateTime.Now.Year
        End If
        eventManager.Cascade(AggregateHeader_DB.TableAggregateHeader + ".YearForAggr")

        If m_Helper.AreaType = StatisticAreaType.None OrElse m_Helper.PeriodType = StatisticPeriodType.None Then
            bv.winclient.Core.MessageForm.Show(m_Helper.EmptyAggregateSettingsMessage)
            m_IsCaseEnabled = False
        End If
        m_BindingDefined = True
        If Not m_IsCaseEnabled Then Me.ReadOnly = True
    End Sub

    Private Sub InitCustomMandatoryFields(caseType As AggregateCaseType)
        If caseType = AggregateCaseType.AggregateCase Then
            MandatoryFieldHelper.SetCustomMandatoryField(cbReceivedInst, CustomMandatoryField.HumanAggrCase_NotificationReceivedByOffice)
            MandatoryFieldHelper.SetCustomMandatoryField(cbReceivedBy, CustomMandatoryField.HumanAggrCase_NotificationReceivedByOfficer)
            MandatoryFieldHelper.SetCustomMandatoryField(dtReceivedDate, CustomMandatoryField.HumanAggrCase_NotificationReceivedByDate)
        ElseIf caseType = AggregateCaseType.VetAggregateCase Then
            MandatoryFieldHelper.SetCustomMandatoryField(cbReceivedInst, CustomMandatoryField.VetAggrCase_NotificationReceivedByOffice)
            MandatoryFieldHelper.SetCustomMandatoryField(cbReceivedBy, CustomMandatoryField.VetAggrCase_NotificationReceivedByOfficer)
            MandatoryFieldHelper.SetCustomMandatoryField(dtReceivedDate, CustomMandatoryField.VetAggrCase_NotificationReceivedByDate)
        ElseIf caseType = AggregateCaseType.VetAggregateAction Then
            MandatoryFieldHelper.SetCustomMandatoryField(cbReceivedInst, CustomMandatoryField.VetAggrAction_NotificationReceivedByOffice)
            MandatoryFieldHelper.SetCustomMandatoryField(cbReceivedBy, CustomMandatoryField.VetAggrAction_NotificationReceivedByOfficer)
            MandatoryFieldHelper.SetCustomMandatoryField(dtReceivedDate, CustomMandatoryField.VetAggrAction_NotificationReceivedByDate)
        End If
    End Sub

    'Private m_CurrentTimeInterval As StatisticPeriodType = StatisticPeriodType.None

    Private Sub BindTime()
        If Not Utils.IsEmpty(m_Helper.CurrentDay) Then
            m_Helper.CurrentTimeInterval = StatisticPeriodType.Day
        ElseIf m_Helper.CurrentWeek > 0 Then
            m_Helper.CurrentTimeInterval = StatisticPeriodType.Week
        ElseIf m_Helper.CurrentMonth > 0 Then
            m_Helper.CurrentTimeInterval = StatisticPeriodType.Month
        ElseIf m_Helper.CurrentQuarter > 0 Then
            m_Helper.CurrentTimeInterval = StatisticPeriodType.Quarter
        ElseIf m_Helper.CurrentYear > 0 Then
            m_Helper.CurrentTimeInterval = StatisticPeriodType.Year
        End If
        If m_Helper.PeriodType <> StatisticPeriodType.None Then
            If m_Helper.CurrentTimeInterval <> StatisticPeriodType.None AndAlso m_Helper.PeriodType <> m_Helper.CurrentTimeInterval Then
                m_IsCaseEnabled = False
            Else
                m_IsCaseEnabled = True
                m_Helper.CurrentTimeInterval = m_Helper.PeriodType
            End If
        End If
        m_Helper.SetYear()
        lblYear.Visible = False
        SetMandatoryFieldVisible(cbYear, False)
        lblQuarter.Visible = False
        SetMandatoryFieldVisible(cbQuarter, False)
        lblMonth.Visible = False
        SetMandatoryFieldVisible(cbMonth, False)
        lblWeek.Visible = False
        SetMandatoryFieldVisible(cbWeek, False)
        lblDay.Visible = False
        SetMandatoryFieldVisible(datDay, False)
        Select Case m_Helper.CurrentTimeInterval
            Case StatisticPeriodType.Year
                lblYear.Visible = True
                SetMandatoryFieldVisible(cbYear, True)
            Case StatisticPeriodType.Quarter
                lblYear.Visible = True
                SetMandatoryFieldVisible(cbYear, True)
                lblQuarter.Visible = True
                SetMandatoryFieldVisible(cbQuarter, True)
            Case StatisticPeriodType.Month
                lblYear.Visible = True
                SetMandatoryFieldVisible(cbYear, True)
                lblMonth.Visible = True
                SetMandatoryFieldVisible(cbMonth, True)
            Case StatisticPeriodType.Week
                lblYear.Visible = True
                SetMandatoryFieldVisible(cbYear, True)
                lblWeek.Visible = True
                SetMandatoryFieldVisible(cbWeek, True)
            Case StatisticPeriodType.Day
                lblYear.Visible = True
                SetMandatoryFieldVisible(cbYear, True)
                lblMonth.Visible = True
                SetMandatoryFieldVisible(cbMonth, True)
                lblDay.Visible = True
                SetMandatoryFieldVisible(datDay, True)
        End Select
    End Sub
    'Private m_CurrentAdminLevel As StatisticAreaType = StatisticAreaType.None
    Private Sub BindArea()
        If (Not Utils.IsEmpty(CurrentRow("idfsCountry"))) AndAlso _
           (Not AggregateHeaderDbService Is Nothing) AndAlso (Not AggregateHeaderDbService.IsNewObject) Then
            If Utils.Str(CurrentRow("idfsCountry")) = _
               Utils.Str(CurrentRow("idfsAdministrativeUnit")) Then
                m_Helper.CurrentAdminLevel = StatisticAreaType.Country
            ElseIf Utils.Str(CurrentRow("idfsRegion")) = _
                   Utils.Str(CurrentRow("idfsAdministrativeUnit")) Then
                m_Helper.CurrentAdminLevel = StatisticAreaType.Region
            ElseIf Utils.Str(CurrentRow("idfsRayon")) = _
                   Utils.Str(CurrentRow("idfsAdministrativeUnit")) Then
                m_Helper.CurrentAdminLevel = StatisticAreaType.Rayon
            ElseIf Utils.Str(CurrentRow("idfsSettlement")) = _
                   Utils.Str(CurrentRow("idfsAdministrativeUnit")) Then
                m_Helper.CurrentAdminLevel = StatisticAreaType.Settlement
            End If
        End If
        If m_Helper.AreaType <> StatisticAreaType.None Then
            If (m_Helper.CurrentAdminLevel <> StatisticAreaType.None) AndAlso (m_Helper.AreaType <> m_Helper.CurrentAdminLevel) Then
                m_IsCaseEnabled = False
            Else
                'CaseEnabled = True
                m_Helper.CurrentAdminLevel = m_Helper.AreaType
            End If
        End If
        m_Helper.BindAreaFields()
        'Core.LookupBinder.BindCountryLookup(cbCountry, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".idfsCountry")
        'Core.LookupBinder.BindRegionLookup(cbRegion, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".idfsRegion")
        'Core.LookupBinder.BindRayonLookup(cbRayon, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".idfsRayon")
        'Core.LookupBinder.BindSettlementLookup(cbSettlement, baseDataSet, AggregateHeader_DB.TableAggregateHeader + ".idfsSettlement")
        'eventManager.AttachDataHandler(AggregateHeader_DB.TableAggregateHeader + ".idfsCountry", AddressOf Country_Changed)
        'eventManager.AttachDataHandler(AggregateHeader_DB.TableAggregateHeader + ".idfsRegion", AddressOf Region_Changed)
        'eventManager.AttachDataHandler(AggregateHeader_DB.TableAggregateHeader + ".idfsRayon", AddressOf Rayon_Changed)
        'eventManager.AttachDataHandler(AggregateHeader_DB.TableAggregateHeader + ".idfsSettlement", AddressOf Settlement_Changed)
        lblCountry.Visible = False
        SetMandatoryFieldVisible(cbCountry, False)
        lblRegion.Visible = False
        SetMandatoryFieldVisible(cbRegion, False)
        lblRayon.Visible = False
        SetMandatoryFieldVisible(cbRayon, False)
        lblSettlement.Visible = False
        SetMandatoryFieldVisible(cbSettlement, False)
        Select Case m_Helper.CurrentAdminLevel
            Case StatisticAreaType.Country
                lblCountry.Visible = True
                SetMandatoryFieldVisible(cbCountry, True)
                cbCountry.Enabled = False
            Case StatisticAreaType.Region
                lblCountry.Visible = True
                SetMandatoryFieldVisible(cbCountry, True)
                cbCountry.Enabled = False
                lblRegion.Visible = True
                SetMandatoryFieldVisible(cbRegion, True)
            Case StatisticAreaType.Rayon
                lblCountry.Visible = True
                SetMandatoryFieldVisible(cbCountry, True)
                cbCountry.Enabled = False
                lblRegion.Visible = True
                SetMandatoryFieldVisible(cbRegion, True)
                lblRayon.Visible = True
                SetMandatoryFieldVisible(cbRayon, True)
            Case StatisticAreaType.Settlement
                lblCountry.Visible = True
                SetMandatoryFieldVisible(cbCountry, True)
                cbCountry.Enabled = False
                lblRegion.Visible = True
                SetMandatoryFieldVisible(cbRegion, True)
                lblRayon.Visible = True
                SetMandatoryFieldVisible(cbRayon, True)
                lblSettlement.Visible = True
                SetMandatoryFieldVisible(cbSettlement, True)
        End Select
        eventManager.Cascade(AggregateHeader_DB.TableAggregateHeader + ".idfsCountry")
    End Sub


#End Region
#Region "Properties"
    '<Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    'Public ReadOnly Property AreaType() As StatisticAreaType
    '    Get
    '        If baseDataSet.Tables(AggregateHeader_DB.TableSettings).Rows.Count <> 1 OrElse baseDataSet.Tables(AggregateHeader_DB.TableSettings).Rows(0)("idfsStatisticAreaType") Is DBNull.Value Then
    '            Return StatisticAreaType.None
    '        End If
    '        Return CType(baseDataSet.Tables(AggregateHeader_DB.TableSettings).Rows(0)("idfsStatisticAreaType"), StatisticAreaType)
    '    End Get
    'End Property
    '<Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    'Public ReadOnly Property PeriodType() As StatisticPeriodType
    '    Get
    '        If baseDataSet.Tables(AggregateHeader_DB.TableSettings).Rows.Count <> 1 OrElse baseDataSet.Tables(AggregateHeader_DB.TableSettings).Rows(0)("idfsStatisticPeriodType") Is DBNull.Value Then
    '            Return StatisticPeriodType.None
    '        End If
    '        Return CType(baseDataSet.Tables(AggregateHeader_DB.TableSettings).Rows(0)("idfsStatisticPeriodType"), StatisticPeriodType)
    '    End Get
    'End Property
    '<Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    'Public ReadOnly Property CaseType() As AggregateCaseType
    '    Get
    '        If CurrentRow("idfsAggrCaseType") Is Nothing Then
    '            Return AggregateCaseType.None
    '        Else
    '            Return CType(CurrentRow("idfsAggrCaseType"), AggregateCaseType)
    '        End If
    '    End Get
    'End Property
    '<Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    'Private Property CurrentDay() As Object
    '    Get
    '        Return CurrentRow("DayForAggr")
    '    End Get
    '    Set(ByVal value As Object)
    '        CurrentRow("DayForAggr") = value
    '    End Set
    'End Property
    '<Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    'Private Property CurrentMonth() As Integer
    '    Get
    '        If CurrentRow("MonthForAggr") Is DBNull.Value Then
    '            Return -1
    '        End If
    '        Return CInt(CurrentRow("MonthForAggr"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        If value > 0 Then
    '            CurrentRow("MonthForAggr") = value
    '        Else
    '            CurrentRow("MonthForAggr") = DBNull.Value
    '        End If
    '    End Set
    'End Property
    '<Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    'Private Property CurrentQuarter() As Integer
    '    Get
    '        If CurrentRow("QuarterForAggr") Is DBNull.Value Then
    '            Return -1
    '        End If
    '        Return CInt(CurrentRow("QuarterForAggr"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        If value > 0 Then
    '            CurrentRow("QuarterForAggr") = value
    '        Else
    '            CurrentRow("QuarterForAggr") = DBNull.Value
    '        End If
    '    End Set
    'End Property
    '<Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    'Private Property CurrentWeek() As Integer
    '    Get
    '        If CurrentRow("WeekForAggr") Is DBNull.Value Then
    '            Return -1
    '        End If
    '        Return CInt(CurrentRow("WeekForAggr"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        If value > 0 Then
    '            CurrentRow("WeekForAggr") = value
    '        Else
    '            CurrentRow("WeekForAggr") = DBNull.Value
    '        End If
    '    End Set
    'End Property
    '<Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    'Private Property CurrentYear() As Integer
    '    Get
    '        If CurrentRow("YearForAggr") Is DBNull.Value Then
    '            Return -1
    '        End If
    '        Return CInt(CurrentRow("YearForAggr"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        If value > 0 Then
    '            CurrentRow("YearForAggr") = value
    '        Else
    '            CurrentRow("YearForAggr") = DBNull.Value
    '        End If
    '    End Set
    'End Property
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property IsCaseEnabled() As Boolean
        Get
            Return m_IsCaseEnabled
        End Get
    End Property
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property CanEdit() As Boolean
        Get
            Return m_CanEdit
        End Get
    End Property

#End Region
#Region "Field Handlers"
    Private Sub SentByOffice_Changed(ByVal sender As Object, ByVal e As DataFieldChangeEventArgs)
        CurrentRow("idfSentByPerson") = DBNull.Value
        CurrentRow.EndEdit()
        Core.LookupBinder.SetPersonFilter(e.Row, "idfSentByOffice", cbRepBy, True)
    End Sub

    Private Sub ReceivedByOffice_Changed(ByVal sender As Object, ByVal e As DataFieldChangeEventArgs)
        CurrentRow("idfReceivedByPerson") = DBNull.Value
        CurrentRow.EndEdit()
        Core.LookupBinder.SetPersonFilter(e.Row, "idfReceivedByOffice", cbReceivedBy, True)
    End Sub
    Private Sub ResetPersonOrganization(personId As Object, row As DataRow, orgFieldName As String)
        Dim organizationId As Object = LookupCache.GetLookupValue(personId, LookupTables.Person, "idfOffice")
        If Not Utils.IsEmpty(organizationId) AndAlso Not organizationId.Equals(row(orgFieldName)) Then
            row(orgFieldName) = organizationId
            row.EndEdit()
        End If

    End Sub
    Private Sub SentByPerson_Changed(ByVal sender As Object, ByVal e As DataFieldChangeEventArgs)
        ResetPersonOrganization(e.Value, e.Row, "idfSentByOffice")
    End Sub

    Private Sub ReceivedByPerson_Changed(ByVal sender As Object, ByVal e As DataFieldChangeEventArgs)
        ResetPersonOrganization(e.Value, e.Row, "idfReceivedByOffice")
    End Sub


#End Region
    Public Overrides Function ValidateData() As Boolean
        Dim res As Boolean = True
        If MyBase.ValidateData = False Then
            Return False
        End If
        If AggregateHeaderDbService.AggregateCaseExists(baseDataSet.Tables(AggregateHeader_DB.TableAggregateHeader).Rows(0)) Then
            Dim errMsg As String = String.Empty
            Dim row As DataRow = baseDataSet.Tables(AggregateHeader_DB.TableAggregateHeader).Rows(0)
            Dim adminUnit As String
            If (cbSettlement.Visible) Then
                adminUnit = cbSettlement.Text
            ElseIf cbRayon.Visible Then
                adminUnit = cbRayon.Text
            ElseIf cbRegion.Visible Then
                adminUnit = cbRegion.Text
            Else
                adminUnit = cbCountry.Text
            End If
            Dim period As String = String.Format("{0:d} - {1:d}", row("datStartDate"), row("datFinishDate"))
            Select Case Me.m_Helper.CaseType
                Case AggregateCaseType.VetAggregateCase
                    errMsg = String.Format(EidssMessages.Get("mbDoubleVetAggregateCase"), period, adminUnit)
                Case AggregateCaseType.VetAggregateAction
                    errMsg = String.Format(EidssMessages.Get("mbDoubleVetAggregateAction"), period, adminUnit)
                Case AggregateCaseType.AggregateCase
                    errMsg = String.Format(EidssMessages.Get("mbDoubleHumanAggregateCase"), period, adminUnit)
            End Select
            ErrorForm.ShowMessageDirect(errMsg)
            Return False
        End If
        Return True
    End Function
    Public Function CaseCanBeChanged(ByVal sender As Object) As Boolean
        Dim res As Boolean = False
        If Not m_CanEdit Then
            Dim activeCtrl As Object = Nothing
            If (TypeOf (sender) Is DevExpress.XtraEditors.BaseEdit) AndAlso _
               (Not CType(sender, DevExpress.XtraEditors.BaseEdit).GetContainerControl Is Nothing) AndAlso _
               (Not CType(sender, DevExpress.XtraEditors.BaseEdit).GetContainerControl.ActiveControl Is Nothing) Then
                activeCtrl = CType(sender, DevExpress.XtraEditors.BaseEdit).GetContainerControl.ActiveControl
            Else
                activeCtrl = sender
            End If
            If Me.ActiveControl Is activeCtrl Then
                If Utils.Str(CurrentRow("strCaseID")) = "" OrElse Utils.Str(CurrentRow("strCaseID")) = "(new)" Then
                    res = True
                    m_CanEdit = True
                Else
                    If m_IsCaseEnabled Then
                        'Dim dialogRes As DialogResult = _
                        '    bv.winclient.Core.MessageForm.Show(EidssMessages.Get("msgChangeData", "Do you want to change data?"), BvMessages.Get("Warning"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        'If dialogRes = DialogResult.Yes Then
                        res = True
                        m_CanEdit = True
                        'End If
                    End If
                End If
            Else
                res = True
            End If
        Else
            res = True
        End If
        Return res
    End Function

    Private Sub datDay_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles datDay.EditValueChanging
        'If OkToChangeDay Then
        If (Not m_CanEdit) AndAlso (Not Me.ActiveControl Is cbMonth) AndAlso Not CaseCanBeChanged(sender) Then
            e.Cancel = True
        End If
        If (Not Me.ActiveControl Is Nothing) AndAlso m_Helper.CurrentTimeInterval = StatisticPeriodType.Day AndAlso Not Utils.IsEmpty(e.NewValue) Then
            If Not Utils.IsEmpty(m_Helper.CurrentMonth) Then
                Dim NewDay As Date = Nothing
                If (TypeOf (e.NewValue) Is Date) Then
                    NewDay = CDate(e.NewValue)
                Else
                    NewDay = Date.Parse(Utils.Str(e.NewValue), System.Globalization.CultureInfo.InvariantCulture)
                End If
                Dim r As DataRow = m_Helper.FindMonth(m_Helper.CurrentMonth)
                If NewDay.CompareTo(r("StartDay")) < 0 OrElse NewDay.CompareTo(r("FinishDay")) > 0 Then
                    e.Cancel = True
                End If
            Else
                e.Cancel = True
            End If
        End If
        'Else
        'e.Cancel = True
        'End If
    End Sub
    Private Function CanChangeControl(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs, ByVal tryCounter As Integer) As Boolean
        If Not m_CanEdit Then
            If (tryCounter < 2) AndAlso (Not Utils.IsEmpty(e.OldValue)) AndAlso Utils.IsEmpty(e.NewValue) Then
                If tryCounter = 1 Then
                    e.Cancel = True
                    tryCounter += 1
                Else
                    e.Cancel = Not CaseCanBeChanged(sender)
                    If e.Cancel Then tryCounter += 1
                End If
            Else
                e.Cancel = Not CaseCanBeChanged(sender)
            End If
        End If
        Return Not e.Cancel
    End Function
    Dim m_TriedToClearReceivedInst As Integer = 0
    Private Sub cbReceivedInst_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs)
        CanChangeControl(sender, e, m_TriedToClearReceivedInst)
    End Sub
    Dim m_TriedToClearRepSource As Integer = 0
    Private Sub cbRepSource_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs)
        CanChangeControl(sender, e, m_TriedToClearRepSource)
    End Sub

    Dim m_TriedToClearRegion As Integer = 0
    Private Sub cbRegion_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles cbRegion.EditValueChanging
        CanChangeControl(sender, e, m_TriedToClearRegion)
    End Sub
    Dim m_TriedToClearRayon As Integer = 0
    Private Sub cbRayon_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles cbRayon.EditValueChanging
        CanChangeControl(sender, e, m_TriedToClearRayon)
    End Sub
    Dim m_TriedToClearSettlement As Integer = 0
    Private Sub cbSettlement_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles cbSettlement.EditValueChanging
        CanChangeControl(sender, e, m_TriedToClearSettlement)
    End Sub

    Private Sub cbYear_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles cbYear.EditValueChanging
        If (Not Utils.IsEmpty(e.NewValue)) AndAlso _
               ((Not TypeOf (e.NewValue) Is Integer) OrElse _
                (CInt(e.NewValue) < Date.MinValue.Year) OrElse _
                (CInt(e.NewValue) > Date.Today.Year)) Then
            e.Cancel = True
        ElseIf (Not Me.ActiveControl Is Nothing) AndAlso (Not m_CanEdit) Then
            e.Cancel = Not CaseCanBeChanged(Me.ActiveControl)
        End If
    End Sub

    Private Sub General_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) _
                                        Handles cbRepSource.EditValueChanging, _
                                                cbCountry.EditValueChanging, cbMonth.EditValueChanging, _
                                                cbQuarter.EditValueChanging, cbWeek.EditValueChanging
        If Not m_CanEdit Then e.Cancel = Not CaseCanBeChanged(sender)
    End Sub
    Private Sub CellValueChanging(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs)
        If (Not CanEdit) AndAlso (Me.State And BusinessObjectState.NewObject) = 0 Then e.Cancel = Not CaseCanBeChanged(Me.ActiveControl)
    End Sub

    Public Sub AttachCellChangingHandler(ByVal ffGrid As FlexibleForms.FFPresenter)
        Dim gridView As GridView = AggregateHelper.GetGridView(ffGrid)
        If Not gridView Is Nothing Then
            AddHandler gridView.ShownEditor, AddressOf ShownEditor
        End If

    End Sub

    Private Sub ShownEditor(ByVal sender As Object, ByVal e As EventArgs)
        If TypeOf sender Is GridView Then
            RemoveHandler CType(sender, GridView).ActiveEditor.EditValueChanging, AddressOf CellValueChanging
            AddHandler CType(sender, GridView).ActiveEditor.EditValueChanging, AddressOf CellValueChanging
        End If
    End Sub

    Public Function ShowFlexibleForm(ByVal ffGrid As FlexibleForms.FFPresenter, ByVal formType As FFType, ByVal templateID As Nullable(Of Long), ByVal matrixVersion As Nullable(Of Long)) As Boolean
        Dim row As DataRow = baseDataSet.Tables(0).Rows(0)
        Dim ObservationField As String = ""
        Dim TemplateField As String = ""
        Dim VersionField As String = ""
        Select Case formType
            Case FFType.HumanAggregateCase, FFType.VetAggregateCase
                ObservationField = "idfCaseObservation"
                TemplateField = "idfsCaseObservationFormTemplate"
                VersionField = "idfVersion"
            Case FFType.VetEpizooticAction
                ObservationField = "idfSanitaryObservation"
                TemplateField = "idfsSanitaryObservationFormTemplate"
                VersionField = "idfSanitaryVersion"
            Case FFType.VetEpizooticActionDiagnosisInv
                ObservationField = "idfDiagnosticObservation"
                TemplateField = "idfsDiagnosticObservationFormTemplate"
                VersionField = "idfDiagnosticVersion"
            Case FFType.VetEpizooticActionTreatment
                ObservationField = "idfProphylacticObservation"
                TemplateField = "idfsProphylacticObservationFormTemplate"
                VersionField = "idfProphylacticVersion"
            Case Else
                Dbg.Fail("unsupported aggregate flexible form type")
        End Select
        If Not templateID.HasValue Then
            templateID = Utils.ToNullableLong(row(TemplateField))
        End If
        If Not matrixVersion.HasValue Then
            matrixVersion = Utils.ToNullableLong(row(VersionField))
        End If
        'matrixVersion = 123010000000
        Dim result As Boolean = AggregateHelper.ShowFlexibleForm(ffGrid, CLng(row(ObservationField)), formType, templateID, matrixVersion)
        If result Then
            row(TemplateField) = ffGrid.TemplateID
            row(VersionField) = ffGrid.VersionMatrixStub
        End If
        Return result
    End Function
    Private m_DateValidator As DateChainValidator
    Public ReadOnly Property DateValidator As DateChainValidator
        Get
            If (m_DateValidator Is Nothing) Then
                m_DateValidator = New DateChainValidator(Me, dtReportingDate, AggregateHeader_DB.TableAggregateHeader, "datSentByDate", EidssFields.Get("lbNotificationSentByDate"), , , , True)
                Dim item As ChainValidator(Of DateTime) = m_DateValidator.AddChild(New DateChainValidator(Me, dtReceivedDate, AggregateHeader_DB.TableAggregateHeader, "datReceivedByDate", EidssFields.Get("lbNotificationReceivedByDate")))
                item.AddChild(New DateChainValidator(Me, dtEnteringDate, AggregateHeader_DB.TableAggregateHeader, "datEnteredByDate", EidssFields.Get("lbNotificationEnteredByDate")))
            End If
            Return m_DateValidator
        End Get
    End Property

End Class
