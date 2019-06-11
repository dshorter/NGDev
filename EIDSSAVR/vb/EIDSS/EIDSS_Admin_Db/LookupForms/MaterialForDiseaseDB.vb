Imports System.Data
Imports System.Data.Common
Imports EIDSS.model.Enums
Imports bv.common.Enums

Public Class MaterialForDiseaseDB
    Inherits BaseDbService

    Public Sub New()
        ObjectName = "MaterialForDisease"
    End Sub

    Public Overrides Function GetDetail(ByVal ID As Object) As DataSet
        Dim ds As New DataSet
        Try
            Dim cmd As IDbCommand = CreateSPCommand("spMaterialForDisease_SelectDetail")
            FillDataset(cmd, ds, "MaterialForDisease")
            CorrectTable(ds.Tables(0), "MaterialForDisease")
            CorrectTable(ds.Tables(1), "MasterDiagnosis")
            ds.Tables("MasterDiagnosis").Columns(0).ReadOnly = False
            ClearColumnsAttibutes(ds)
            ds.Tables("MaterialForDisease").Columns("intRecommendedQty").DefaultValue = 1
            ds.Tables(0).Columns("idfMaterialForDisease").AutoIncrement = True
            ds.Tables(0).Columns("idfMaterialForDisease").AutoIncrementSeed = -1
            ds.Tables(0).Columns("idfMaterialForDisease").AutoIncrementStep = -1
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
            ExecPostProcedure("spMaterialForDisease_Post", ds.Tables("MaterialForDisease"), Connection, transaction)
            db.Core.LookupCache.NotifyChange("rftSampleType", transaction)
            db.Core.LookupCache.NotifyChange(LookupTables.SampleTypeForDiagnosis.ToString(), transaction)
            AddEvent(EventType.MatrixChanged, "MaterialForDiseaseDetail", False)
        Catch ex As Exception
            m_Error = New ErrorMessage(StandardError.PostError, ex)
            Return False
        End Try
        Return True
    End Function

End Class
