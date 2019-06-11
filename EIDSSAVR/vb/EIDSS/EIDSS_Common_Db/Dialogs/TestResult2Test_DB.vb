Imports System.Data
Imports EIDSS.model.Enums
Imports bv.common.Enums

Public Class TestResult2Test_DB
    Inherits BaseDbService
    Public Const TableTestResult2Test As String = "TestResult2Test"
    Public Const TableTest As String = "Test"

    Public Sub New()
        ObjectName = "TestResult2Test"
        UseDatasetCopyInPost = False
    End Sub
    Public Overrides Function GetDetail(ByVal ID As Object) As DataSet
        Dim ds As New DataSet
        Try
            Dim cmd As IDbCommand = CreateSPCommand("spTestResult2Test_SelectDetail")
            AddParam(cmd, "@LangID", bv.model.Model.Core.ModelUserContext.CurrentLanguage)
            ds.EnforceConstraints = False
            FillDataset(cmd, ds, TableTest)
            CorrectTable(ds.Tables(0), TableTest)
            CorrectTable(ds.Tables(1), TableTestResult2Test)
            ds.Tables(TableTestResult2Test).PrimaryKey = New DataColumn() { _
                    ds.Tables(TableTestResult2Test).Columns("idfsTestName"), _
                    ds.Tables(TableTestResult2Test).Columns("idfsTestResult")}
            ClearColumnsAttibutes(ds)
            ds.EnforceConstraints = True
            m_ID = ID
            Return ds
        Catch ex As Exception
            m_Error = New ErrorMessage(StandardError.FillDatasetError, ex)
            Return Nothing
        End Try
    End Function


    Public Overrides Function PostDetail(ByVal ds As DataSet, ByVal postType As Integer, Optional ByVal transaction As IDbTransaction = Nothing) As Boolean
        If ds Is Nothing Then Return True
        Try
            ExecPostProcedure("spTestResult2Test_Post", ds.Tables(TableTestResult2Test), Connection, transaction)
            db.Core.LookupCache.NotifyChange("rftTestResult", transaction)
            db.Core.LookupCache.NotifyChange("rftPensideTestResult", transaction)
            db.Core.LookupCache.NotifyChange("TestResult", transaction)
            AddEvent(EventType.MatrixChanged, "TestResult2TestDetail", False)

        Catch ex As Exception
            m_Error = New ErrorMessage(StandardError.PostError, ex)
            Return False
        End Try
        Return True

    End Function

    Public Shared Function AddTestResult2Test(ByVal ds As DataSet, ByVal idfsTestResult As Long, ByVal testResultName As String, ByVal testKind As Integer) As DataRow
        Dim row As DataRow = Nothing
        Dim rows As DataRow()
        'If result was previously deleted, accept changes for this row to avoid invalid row posting
        rows = ds.Tables(TableTestResult2Test).Select(String.Format("idfsTestResult={0} and idfsTestName={1}", idfsTestResult, ds.Tables(TableTest).Rows(0)("idfsReference")), Nothing, DataViewRowState.Deleted)
        If rows.Length > 0 Then
            For Each row In rows
                row.AcceptChanges()
            Next
        End If
        rows = ds.Tables(TableTestResult2Test).Select(String.Format("idfsTestResult={0} and idfsTestName={1}", idfsTestResult, ds.Tables(TableTest).Rows(0)("idfsReference")))
        ds.Tables(TableTestResult2Test).BeginLoadData()
        If rows.Length = 0 Then
            row = ds.Tables(TableTestResult2Test).NewRow
            row("idfsTestName") = ds.Tables(TableTest).Rows(0)("idfsReference")
            row("idfsTestResult") = idfsTestResult
            row("TestResultName") = testResultName
            row("TestKind") = TestKind
            row("blnIndicative") = 0
            ds.Tables(TableTestResult2Test).Rows.Add(row)
        End If
        ds.Tables(TableTestResult2Test).EndLoadData()
        Return row
    End Function
    Public Shared Function RemoveTestResultFromTest(ByVal ds As DataSet, ByVal idfsTestResult As String) As DataRow
        Dim row As DataRow = Nothing
        Dim rows As DataRow()
        Dim curTestType As Object = ds.Tables(TableTest).Rows(0)("idfsReference")
        rows = ds.Tables(TableTestResult2Test).Select(String.Format("idfsTestResult={0} and idfsTestName={1}", idfsTestResult, curTestType))
        ds.Tables(TableTestResult2Test).BeginLoadData()
        If rows.Length > 0 Then
            rows(0).Delete()
        End If
        ds.Tables(TableTestResult2Test).EndLoadData()
        Return row
    End Function

End Class
