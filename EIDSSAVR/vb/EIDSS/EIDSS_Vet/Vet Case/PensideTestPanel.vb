Imports System.ComponentModel
Imports bv.winclient.Core
Imports EIDSS.model.Resources
Imports bv.common.Resources
Imports bv.winclient.Localization
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid
Imports DevExpress.XtraEditors
Imports bv.common.Enums

Public Class PensideTestPanel
    Protected Overrides Sub DefineBinding()
        If IsDesignMode() Then Return
        Core.LookupBinder.BindStandardRepositoryLookup(cbTestResult, LookupTables.PensideTestResult, AddressOf cbTestResult_Filter, False)
        Core.LookupBinder.BindBaseRepositoryLookup(cbTestType, BaseReferenceType.rftPensideTestType, HACode, False)
        If Not baseDataSet Is Nothing AndAlso baseDataSet.Tables.Count <> 0 Then
            TestGrid.DataSource = baseDataSet.Tables(PensideTests_Db.TableTests)
        End If
        cbSampleBarcode.Columns(0).Caption = EidssMessages.Get("lblSpecimenType")
        cbSampleBarcode.Columns(1).Caption = EidssMessages.Get("lblFieldSampleID")
        UpdateButtons()

    End Sub

    Dim m_ShowPopupImmediatly As Boolean
    Private Sub cmdNewTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not IsTestRowValid(TestGridView.FocusedRowHandle) Then
            Return
        End If
        Dim row As DataRow = PensideTestsDbService.CreateTest(baseDataSet)
        DxControlsHelper.SetRowHandleForDataRow(TestGridView, row, "idfPensideTest")
        TestGridView.FocusedColumn = colTestName
        m_ShowPopupImmediatly = True
        TestGrid.Select()
        Application.DoEvents()
        TestGridView.ShowEditor()
    End Sub
    Private Sub cmdDeleteTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeleteTest.Click
        Dim testRow As DataRow = TestGridView.GetDataRow(TestGridView.FocusedRowHandle)
        If testRow Is Nothing Then Return
        If WinUtils.ConfirmDelete Then
            PensideTestsDbService.DeleteTest(baseDataSet, testRow("idfPensideTest"))
        End If
    End Sub
    Public WriteOnly Property SamplesView() As DataView
        Set(ByVal value As DataView)
            cbSampleBarcode.DataSource = value
        End Set
    End Property

    Private Sub TestGridView_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles TestGridView.CellValueChanged
        UpdateButtons()
        If e.Column Is colSampleBarcode AndAlso TestGridView.FocusedRowHandle = GridControl.NewItemRowHandle Then
            RefreshSample(e.Value)
        End If
    End Sub
    Private Sub TestGridView_CellValueChanging(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles TestGridView.CellValueChanging
        If Me.Created = False Then
            Return
        End If
        If e.Column Is colSampleBarcode Then
            RefreshSample(e.Value)
        End If

    End Sub

    Private Sub BeforeAcceptChanges(ByVal sender As Object, ByVal e As EventArgs)
        'For i As Integer = 0 To baseDataSet.Tables(PensideTests_Db.TableTests).Rows.Count - 1
        '    Dim testRow As DataRow = baseDataSet.Tables(PensideTests_Db.TableTests).Rows(i)
        '    Dim view As DataView = CType(cbSampleBarcode.DataSource, DataView)
        '    If view Is Nothing Then Return
        '    Dim lookupRow As DataRow = Nothing
        '    If Not Utils.IsEmpty(testRow("idfMaterial")) Then
        '        lookupRow = view.Table.Rows.Find(testRow("idfMaterial"))
        '    End If
        '    If Not lookupRow Is Nothing AndAlso Utils.Str(testRow("AnimalID")) <> Utils.Str(lookupRow("AnimalID")) Then
        '        testRow("idfAnimal") = lookupRow("AnimalID")
        '        testRow.EndEdit()
        '    End If
        'Next
    End Sub


    Private Function GetCurrentTestRow() As DataRow
        Return TestGridView.GetFocusedDataRow()
    End Function

    'Private Sub cbTestResult_CloseUp(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles cbTestResult.CloseUp
    '    CType(cbTestResult.DataSource, DataView).RowFilter = ""
    'End Sub
    'Private Sub cbTestResult_QueryPopUp(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cbTestResult.QueryPopUp
    '    Dim view As DataView = CType(cbTestResult.DataSource, DataView)
    '    'try to display results for current test type
    '    Dim testType As Object = TestGridView.GetFocusedRowCellValue(colTestName)
    '    If Not testType Is DBNull.Value Then
    '        view.RowFilter = String.Format("(idfsPensideTestName = {0} and intRowStatus = 0) or idfRowNumber = {1}", testType, LookupCache.EmptyLineKey)
    '    Else
    '        view.RowFilter = String.Format("(idfsPensideTestName = 0 and intRowStatus = 0) or idfRowNumber = {0}", LookupCache.EmptyLineKey) 'display all test results
    '    End If
    '    If view.Count = 0 Then
    '        view.RowFilter = String.Format("(idfsPensideTestName = 0 and intRowStatus = 0) or idfRowNumber = {0}", LookupCache.EmptyLineKey) 'display all test results
    '    End If
    'End Sub
    Private Sub cbTestResult_Filter(ByVal sender As Object, ByVal e As EventArgs)
        'try to display results for current test type
        Dim testType As Object = TestGridView.GetFocusedRowCellValue(colTestName)
        Dim filter As String
        If Not Utils.IsEmpty(testType) Then
            filter = String.Format("(idfsPensideTestName = {0} and intRowStatus = 0) or idfRowNumber = {1}", testType, LookupCache.EmptyLineKey)
        Else
            filter = String.Format("(idfsPensideTestName = 0 and intRowStatus = 0) or idfRowNumber = {0}", LookupCache.EmptyLineKey) 'display all test results
        End If
        Core.LookupBinder.SetDefaultFilter(CType(sender, LookUpEdit), filter, True)
    End Sub
    Public Overrides Function ValidateData() As Boolean
        For i As Integer = 0 To TestGridView.RowCount - 1
            If IsTestRowValid(i, True) = False Then
                Return False
            End If
        Next
        Return True
    End Function
    Private Function GetErrorText(rowHandle As Integer) As String
        Dim row As DataRow = TestGridView.GetDataRow(rowHandle)
        If row Is Nothing Then Return String.Empty
        If row("idfsPensideTestName") Is DBNull.Value Then
            TestGridView.FocusedColumn = TestGridView.Columns("idfsPensideTestName")
            Return StandardErrorHelper.Error(StandardError.Mandatory, TestGridView.Columns("idfsPensideTestName").Caption)
        End If
        If row("idfMaterial") Is DBNull.Value Then
            TestGridView.FocusedColumn = TestGridView.Columns("idfMaterial")
            Return StandardErrorHelper.Error(StandardError.Mandatory, TestGridView.Columns("idfMaterial").Caption)
        End If
        Return String.Empty
    End Function
    Private Function IsTestRowValid(Optional ByVal index As Integer = -1, Optional ByVal showError As Boolean = True) As Boolean
        If index = -1 Then index = TestGridView.FocusedRowHandle
        Dim row As DataRow = TestGridView.GetDataRow(index)

        If row Is Nothing Then Return True
        Dim msg As String = GetErrorText(index)
        If showError AndAlso Not Utils.IsEmpty(msg) Then
            ErrorForm.ShowWarningDirect(msg)
        End If
        Return Utils.IsEmpty(msg)
    End Function
    Dim m_Updating As Boolean
    Private Sub TestGridView_FocusedRowChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles TestGridView.FocusedRowChanged
        If Loading OrElse m_Updating Then Return
        m_Updating = True
        Try
            If e.PrevFocusedRowHandle >= 0 AndAlso IsTestRowValid(e.PrevFocusedRowHandle) = False Then
                TestGridView.FocusedRowHandle = e.PrevFocusedRowHandle
                Return
            End If
        Finally
            UpdateButtons()
            m_Updating = False
        End Try
    End Sub
    Private m_HACode As HACode = HACode.Livestock

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property HACode() As HACode
        Get
            Return m_HACode
        End Get
        Set(ByVal Value As HACode)
            m_HACode = Value
            PensideTestsDbService.HACode = Value
            If (m_HACode And EIDSS.HACode.Livestock) <> 0 Then
                Me.colAnimalID.Visible = True
                Me.colSpecies.Visible = False
            Else
                Me.colAnimalID.Visible = False
                Me.colSpecies.Visible = True
            End If
        End Set
    End Property
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property PartyList() As Object
        Get
            If (HACode And HACode.Livestock) <> 0 Then
                Return cbAnimal.DataSource
            ElseIf (HACode And HACode.Avian) <> 0 Then
                Return cbSpecies.DataSource
            End If
            Return Nothing
        End Get
        Set(ByVal Value As Object)
            If (HACode And HACode.Livestock) <> 0 Then
                cbAnimal.DataSource = Value
            ElseIf (HACode And HACode.Avian) <> 0 Then
                cbSpecies.DataSource = Value
            End If
        End Set
    End Property
    Public Function CanDeleteSample(ByVal sampleID As Object) As Boolean
        If Utils.IsEmpty(sampleID) Then Return False
        For Each row As DataRow In baseDataSet.Tables(PensideTests_Db.TableTests).Rows
            If row.RowState <> DataRowState.Deleted Then
                If row("idfMaterial").Equals(sampleID) Then
                    Return False
                End If
            End If
        Next
        Return True
    End Function

    Public Function CanDeleteParty(ByVal partyID As Object) As Boolean
        If Utils.IsEmpty(partyID) Then Return False
        For Each row As DataRow In baseDataSet.Tables(PensideTests_Db.TableTests).Rows
            If row.RowState <> DataRowState.Deleted Then
                If row("idfParty").Equals(partyID) Then
                    Return False
                End If
            End If
        Next
        Return True
    End Function
    Public Function CanDeleteFarmTreeNode(ByVal row As DataRow) As Boolean
        Dim dtype As PartyType = CType(row("idfsPartyType"), PartyType)
        Select Case dtype
            Case PartyType.Farm
                Return False
            Case PartyType.Herd
                Dim speciesView As DataView = New DataView(row.Table)
                speciesView.RowFilter = String.Format("idfParentParty = {0}", row("idfParty").ToString)
                For Each speciesRow As DataRowView In speciesView
                    If Not CanDeleteFarmTreeNode(speciesRow.Row) Then
                        Return False
                    End If
                Next
            Case PartyType.Species
                Return CanDeleteParty(row("idfParty"))
        End Select
        Return True
    End Function
    ''' <summary>
    ''' This method is called if we select new sample for the test  
    ''' and we should update displayed sample information for current test row
    ''' </summary>
    ''' <param name="newSampleID"></param>
    ''' <remarks></remarks>
    Public Sub RefreshSample(ByVal newSampleID As Object)
        Dim view As DataView = CType(cbSampleBarcode.DataSource, DataView)
        If view Is Nothing Then Return
        Dim lookupRow As DataRow = Nothing
        If Not Utils.IsEmpty(newSampleID) Then
            lookupRow = view.Table.Rows.Find(newSampleID)
        End If
        Dim testRow As DataRow = GetCurrentTestRow()
        If Not testRow Is Nothing Then
            UpdateSampleInfo(testRow, lookupRow)
            Return
        End If
    End Sub

    ''' <summary>
    ''' This method is called when we change sample type in the samples panel
    ''' and we should update all tests related with modified sample
    ''' </summary>
    ''' <param name="oldSampleID">
    ''' the original sample ID. We should update all tests, related with this sample
    ''' </param>
    ''' <param name="newSampleID">
    ''' new sample ID, we should update test rows with this sample info
    ''' </param>
    ''' <remarks></remarks>
    Public Sub RefreshTests(ByVal oldSampleID As Object, ByVal newSampleID As Object)
        If Utils.IsEmpty(oldSampleID) Then ' new sample is created, do nothing
            Return
        End If
        Dim view As DataView = CType(cbSampleBarcode.DataSource, DataView)
        If view Is Nothing Then Return
        Dim lookupRow As DataRow = Nothing
        If Not Utils.IsEmpty(newSampleID) Then
            lookupRow = view.Table.Rows.Find(newSampleID)
        End If
        For Each testRow As DataRow In baseDataSet.Tables(PensideTests_Db.TableTests).Rows
            If testRow("idfMaterial").Equals(oldSampleID) Then
                UpdateSampleInfo(testRow, lookupRow)
            End If
        Next
    End Sub

    Private Sub UpdateSampleInfo(ByVal testRow As DataRow, ByVal sampleRow As DataRow)
        If Not sampleRow Is Nothing Then
            testRow("strSampleName") = Utils.Str(sampleRow("strSampleName"))
            testRow("idfMaterial") = sampleRow("idfMaterial")
            testRow("idfParty") = sampleRow("idfParty")
        Else
            testRow("strSampleName") = DBNull.Value
            testRow("idfMaterial") = DBNull.Value
            testRow("idfParty") = DBNull.Value
        End If
        testRow.EndEdit()
        TestGridView.RefreshRow(TestGridView.FocusedRowHandle)
    End Sub
    Private Sub TestGridView_InitNewRow(sender As Object, e As InitNewRowEventArgs) Handles TestGridView.InitNewRow
        Dim row As DataRow = TestGridView.GetDataRow(e.RowHandle)
        PensideTestsDbService.InitTestRow(row)
        TestGridView.PostEditor()
    End Sub

    Public Sub UpdateButtons()
        Dim rowSelected As Boolean = TestGridView.FocusedRowHandle <> GridControl.NewItemRowHandle AndAlso Not TestGridView.GetFocusedDataRow() Is Nothing
        cmdDeleteTest.Enabled = Not [ReadOnly] AndAlso rowSelected
        TestGridView.OptionsBehavior.Editable = Not [ReadOnly]
        EnableRowAdding((Not [ReadOnly] AndAlso IsTestRowValid(, False)))
    End Sub

    Public Sub EnableRowAdding(enable As Boolean)
        If (TestGridView.FocusedRowHandle = GridControl.NewItemRowHandle) Then
            Return
        End If
        If (Not enable) Then
            TestGridView.OptionsView.NewItemRowPosition = NewItemRowPosition.None
        Else
            TestGridView.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
        End If
    End Sub
    Public Overrides Property [ReadOnly]() As Boolean
        Get
            Return MyBase.ReadOnly
        End Get
        Set(ByVal Value As Boolean)
            MyBase.ReadOnly = Value
            UpdateButtons()
        End Set
    End Property

    Private Sub cbSpecimenID_EditValueChanged(sender As Object, e As System.EventArgs) Handles cbSpecimenID.EditValueChanged
    End Sub

    Private Sub cbSampleBarcode_EditValueChanged(sender As Object, e As EventArgs) Handles cbSampleBarcode.EditValueChanged
        TestGridView.PostEditor()
        RefreshSample(CType(sender, LookUpEdit).EditValue)

    End Sub
    Private ReadOnly Property GridLayoutName As String
        Get
            If (HACode And HACode.Livestock) > 0 Then
                Return "LivestockCase_FieldTests"
            Else
                Return "AvianCase_FieldTests"
            End If
        End Get
    End Property
    Protected Overrides Sub SaveGridLayouts()
        TestGridView.SaveGridLayout(GridLayoutName)
    End Sub
    Protected Overrides Sub LoadGridLayouts()
        'Test Name, Field Sample ID, Result
        TestGridView.InitXtraGridCustomization(New String() {"idfsPensideTestName", "strFieldBarcode", "idfsPensideTestResult"})
        TestGridView.LoadGridLayout(GridLayoutName)
    End Sub

End Class
