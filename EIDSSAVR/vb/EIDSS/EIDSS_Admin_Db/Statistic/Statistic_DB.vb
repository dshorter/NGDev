Imports System.Data
Imports System.Collections.Generic
Imports bv.common.db.Core
Imports bv.common.Core
Imports bv.common.Enums

Public Class Statistic_DB
    Inherits BaseDbService

    Public Sub New()
        ObjectName = "Statistic"
    End Sub

    Public Overrides Function GetDetail(ByVal ID As Object) As DataSet
        Dim ds As New DataSet
        Try
            Dim cmd As IDbCommand = CreateSPCommand("spStatistic_SelectDetail")
            AddParam(cmd, "@idfStatistic", ID)
            AddParam(cmd, "@LangID", bv.model.Model.Core.ModelUserContext.CurrentLanguage)
            FillDataset(cmd, ds, "Statistic")
            CorrectTable(ds.Tables(0), "Statistic", "idfStatistic")
            ClearColumnsAttibutes(ds)
            If ID Is Nothing Then
                ID = NewIntID()
            End If
            If ds.Tables(0).Rows.Count = 0 Then
                m_IsNewObject = True
                Dim r As DataRow = ds.Tables("Statistic").NewRow
                r("idfStatistic") = ID
                ds.EnforceConstraints = False
                ds.Tables("Statistic").Rows.Add(r)
            End If
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
            ExecPostProcedure("spStatistic_Post", ds.Tables("Statistic"), Connection, transaction)
            db.Core.LookupCache.NotifyChange("Statistic", transaction)
        Catch ex As Exception
            m_Error = New ErrorMessage(StandardError.PostError, ex)
            Return False
        End Try
        Return True
    End Function
    Public Function StatisticExists(ByVal row As DataRow) As Boolean
        Dim cmd As IDbCommand = CreateSPCommand("spStatistic_Exists", Connection)
        Dim params As New Dictionary(Of String, Object)
        params.Add("@Area", row("idfsArea"))
        params.Add("@StatisticDataType", row("idfsStatisticDataType"))
        params.Add("@StatisticPeriodType", row("idfsStatisticPeriodType"))
        params.Add("@StartDate", row("datStatisticStartDate"))
        params.Add("@Statistic", row("idfStatistic"))
        params.Add("@idfsMainBaseReference", row("idfsMainBaseReference"))
        params.Add("@idfsStatisticalAgeGroup", row("idfsStatisticalAgeGroup"))
        StoredProcParamsCache.CreateParameters(cmd, params)
        ExecCommand(cmd, Connection)
        Return CBool(GetParamValue(cmd, "@RETURN_VALUE"))
    End Function
    Public Function GetStatisticDataTypeRow(ByVal typeID As Object) As DataRow
        If Utils.IsEmpty(typeID) Then Return Nothing
        Dim cmd As IDbCommand = CreateSPCommand("spStatistic_GetDataType", Connection)
        AddParam(cmd, "@idfsStatisticDataType", typeID)
        Dim dt As New DataTable
        FillTable(cmd, dt)
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

End Class
