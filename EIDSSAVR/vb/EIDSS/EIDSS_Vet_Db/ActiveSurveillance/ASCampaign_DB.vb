Imports EIDSS.ActiveSurveillance
Imports bv.common.db.Core
Imports bv.common.Core
Imports System.Data.SqlClient
Imports EIDSS.model.Enums
Imports bv.common.Enums

Public Class ASCampaign_DB
    Inherits BaseDbService
    Public Const TableCampaign As String = "AsCampaign"
    Public Const TableDiagnosis As String = "Diagnosis"
    Public Const TableSessions As String = "Sessions"
    Public Sub New()
        ObjectName = "AsCampaign"
        UseDatasetCopyInPost = True

    End Sub
    Public Overrides Function GetDetail(ByVal ID As Object) As DataSet
        Dim ds As New DataSet
        Try
            Dim cmd As IDbCommand = CreateSPCommand("spASCampaign_SelectDetail")
            AddParam(cmd, "@idfCampaign", ID)
            AddParam(cmd, "@LangID", bv.model.Model.Core.ModelUserContext.CurrentLanguage)

            FillDataset(cmd, ds, TableCampaign)
            CorrectTable(ds.Tables(0), TableCampaign)
            CorrectTable(ds.Tables(1), TableDiagnosis)
            CorrectTable(ds.Tables(2), TableSessions)
            ClearColumnsAttibutes(ds)
            If ID Is Nothing Then
                ID = NewIntID()
                Dim r As DataRow = ds.Tables(TableCampaign).NewRow
                r("idfCampaign") = ID
                r("strCampaignID") = "(new)"
                r("idfsCampaignStatus") = CLng(ASCampaignStatus.[New])
                ds.Tables(TableCampaign).Rows.Add(r)
                ds.EnforceConstraints = False
                m_IsNewObject = True
            Else
                m_IsNewObject = False
            End If
            ds.Tables(TableDiagnosis).Columns("idfCampaign").DefaultValue = ID
            Dim order As Object = ds.Tables(TableDiagnosis).Compute("max(intOrder)", "")
            If Utils.IsEmpty(order) Then
                order = 0
            Else
                order = CInt(order) + 1
            End If
            'ds.Tables(TableDiagnosis).Columns("idfCampaignToDiagnosis").AutoIncrementSeed = -1
            'ds.Tables(TableDiagnosis).Columns("idfCampaignToDiagnosis").AutoIncrementStep = -1
            'ds.Tables(TableDiagnosis).Columns("idfCampaignToDiagnosis").AutoIncrement = True
            ds.Tables(TableDiagnosis).Columns("intOrder").AutoIncrementSeed = CInt(order)
            ds.Tables(TableDiagnosis).Columns("intOrder").AutoIncrementStep = 1
            ds.Tables(TableDiagnosis).Columns("intOrder").AutoIncrement = True

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
            If IsNewObject Then
                m_IsNewObject = False
                AddEvent(EventType.AsCampaignCreatedLocal, , False)
            ElseIf ds.Tables(TableCampaign).Rows.Count > 0 Then
                Dim r As DataRow = ds.Tables(TableCampaign).Rows(0)
                If r.HasVersion(DataRowVersion.Original) Then
                    If Utils.Str(r("idfsCampaignStatus")) <> _
                       Utils.Str(r("idfsCampaignStatus", DataRowVersion.Original)) Then
                        AddEvent(EventType.AsCampaignStatusChangedLocal, , False)
                    End If
                End If
            End If
            ExecPostProcedure("spASCampaign_Post", ds.Tables(TableCampaign), Connection, transaction)
            ExecPostProcedure("spASCampaignDiagnosis_Post", ds.Tables(TableDiagnosis), Connection, transaction)
            ExecPostProcedure("spASCampaignSession_Post", ds.Tables(TableSessions), Connection, transaction)
            Dim cmd As IDbCommand = CreateSPCommand("spAsCampaign_ValidateDiagnosis", transaction)
            StoredProcParamsCache.CreateParameters(cmd)
            SetParam(cmd, "@idfCampaign", ds.Tables(TableCampaign).Rows(0)("idfCampaign"))
            ExecCommand(cmd, Connection, transaction, True)
        Catch e As SqlException
            If (e.Number = 50000 AndAlso e.Class = 16) Then
                m_Error = New ErrorMessage(e.Message)
            Else
                m_Error = New ErrorMessage(StandardError.PostError, e)
            End If
            Return False
        Catch ex As Exception
            m_Error = New ErrorMessage(StandardError.PostError, ex)
            Return False
        End Try
        Return True

    End Function

    Public Function CanCloseCampaign() As Boolean
        Dim cmd As IDbCommand = CreateSPCommand("spASCampaign_CanClose")
        StoredProcParamsCache.CreateParameters(cmd)
        SetParam(cmd, "@idfCampaign", m_ID)
        m_Error = ExecCommand(cmd, Connection)
        If m_Error Is Nothing Then
            Return CType(GetParamValue(cmd, "@Result"), Boolean)
        End If
        Return False
    End Function

    Public Function CanDeleteDiagnosis(ByVal diagnosis As Long) As Boolean
        Dim cmd As IDbCommand = CreateSPCommand("spASCampaign_CanRemoveDiagnosis")
        StoredProcParamsCache.CreateParameters(cmd)
        SetParam(cmd, "@idfCampaign", m_ID)
        SetParam(cmd, "@idfsDiagnosis", diagnosis)
        m_Error = ExecCommand(cmd, Connection)
        If m_Error Is Nothing Then
            Return CType(GetParamValue(cmd, "@Result"), Boolean)
        End If
        Return False
    End Function
    Public Function HasMonitoringSession() As Boolean
        Dim cmd As IDbCommand = CreateSPCommand("spASCampaign_HasSession")
        StoredProcParamsCache.CreateParameters(cmd)
        SetParam(cmd, "@idfCampaign", m_ID)
        m_Error = ExecCommand(cmd, Connection)
        If m_Error Is Nothing Then
            Return CType(GetParamValue(cmd, "@Result"), Boolean)
        End If
        Return False
    End Function

    Sub RefreshSessionInfo(ds As DataSet)
        If ds.Tables(TableCampaign).Rows.Count = 0 Then Return

        Dim dsNew As DataSet = GetDetail(ds.Tables(TableCampaign).Rows(0)("idfCampaign"))
        ds.Merge(dsNew.Tables(TableSessions))
     End Sub
    Public Function CanAddSessionToCampaign(campaignDiseaseTable As DataTable, sessionId As Long) As Integer
        Dim cmd As IDbCommand = CreateSPCommand("spAsSession_GetUsedSpeciesSampleTypes")
        AddParam(cmd, "@idfMonitoringSession", sessionId)
        Using usedSpeciesSamplesTable As DataTable = ExecTable(cmd)
            If Not AsHelper.ValidateSessionDataAgainstDiseaseRecords(usedSpeciesSamplesTable, campaignDiseaseTable) Then
                Return 1
            End If
        End Using
        cmd = CreateSPCommand("spAsSession_SelectDiagnosis", ConnectionManager.DefaultInstance.Connection)
        StoredProcParamsCache.CreateParameters(cmd)
        SetParam(cmd, "@idfMonitoringSession", sessionId)
        Using sessionDiseaseTable As DataTable = ExecTable(cmd)
            If Not AsHelper.IsSessionDiseasesIncludedToCampaignDesieseas(sessionDiseaseTable, campaignDiseaseTable) Then
                Return 2
            End If
        End Using
        Return 0
    End Function
End Class

