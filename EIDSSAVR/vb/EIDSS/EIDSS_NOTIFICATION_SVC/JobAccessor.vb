Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.SqlServer.Management.Smo
Imports System.Data.SqlClient
Imports bv.common.Diagnostics
Imports System.Threading
Imports EIDSS.model.Core
Imports EIDSS.model.Enums

Public Class JobAccessor
    Private ReadOnly m_ServerName As String
    Private ReadOnly m_SqlConnection As SqlConnection
    Private ReadOnly m_UserName As String
    Private ReadOnly m_Password As String
    Private Const ExpressEditionName As String = "Express Edition"
    Private ReadOnly m_JobQueue As New List(Of IJobProvider)
    Private ReadOnly m_Timer As Timer
    Public Shared Property Instance As JobAccessor
    Public Shared Property FromUnitTest As Boolean = False
    Public Shared Property FromUnitTestSuccess As Boolean = True
    Public Sub New(ByVal serverName As String, ByVal userName As String, ByVal password As String)
        m_ServerName = serverName
        m_UserName = userName
        m_Password = password
        Dim builder As New SqlConnectionStringBuilder()
        builder.DataSource = serverName
        builder.InitialCatalog = "master"
        builder.UserID = userName
        builder.Password = password
        m_SqlConnection = New SqlConnection(builder.ConnectionString)
        m_Timer = New Timer(AddressOf CheckJobsQueueState, Nothing, 1000, 1000)
    End Sub

    Public Function IsJobRuning(jobName As String) As Boolean
        Try
            SyncLock m_JobQueue
                Return m_JobQueue.Any(Function(job) (jobName = job.JobName) AndAlso job.IsRunning)
            End SyncLock
        Catch ex As Exception
            Dbg.Debug("error during job running check: {0}", ex)
        End Try
        Return False
    End Function
    Private Sub CheckJobsQueueState(ByVal state As Object)
        Try
            SyncLock m_JobQueue
                Dim jobsToRemove As New List(Of IJobProvider)
                For Each job As IJobProvider In m_JobQueue
                    If job.IsRunning Then
                        Continue For
                    End If
                    Dim err As String = job.LastError

                    If err Is Nothing Then
                        job.ReplicationController.CreateEvent(model.Enums.EventType.ReplicationSucceeded, Nothing, Nothing)
                        Dbg.Debug("[{0:f}] job {1} is finished successfully", CurrentTimeString, job.JobName)
                        SecurityLog.WriteToEventLogDB(Nothing, SecurityAuditEvent.ProcessStop, True, Nothing, Nothing, String.Format("Replication {0} is finished successfully", job.JobName), SecurityAuditProcessType.Replication)
                    Else
                        'm_JobProvider = Nothing
                        job.ReplicationController.CreateEvent(model.Enums.EventType.ReplicationFailed, Nothing, Nothing)
                        Dbg.Debug("[{0:f}] job {1} is failed with error {2}", CurrentTimeString, job.JobName, err)
                        SecurityLog.WriteToEventLogDB(Nothing, SecurityAuditEvent.ProcessStop, False, Nothing, err, String.Format("Replication {0} failed", job.JobName), SecurityAuditProcessType.Replication)
                    End If
                    jobsToRemove.Add(job)
                Next
                For Each job As IJobProvider In jobsToRemove
                    m_JobQueue.Remove(job)
                Next
            End SyncLock

        Catch ex As Exception
            Dbg.Debug("error during job state checking: {0}", ex)
        End Try

    End Sub

    Public ReadOnly Property JobProvider() As IJobProvider
        Get
            Return GetJobProvider()
        End Get
    End Property
    Public ReadOnly Property ServerName() As String
        Get
            Return m_ServerName
        End Get
    End Property

    'Private m_JobProvider As IJobProvider
    Private Function GetJobProvider() As IJobProvider
        'If m_JobProvider Is Nothing Then
        If IsSqlAgentExist(m_SqlConnection) Then
            'm_JobProvider = 
            Return New SqlClentAgentJobProvider(m_ServerName, m_UserName, m_Password)
        Else
            'm_JobProvider = 
            Return New TaskProvider
        End If
        'End If
        'Return m_JobProvider
    End Function

    Private Shared Function IsSqlAgentExist(ByVal cn As SqlConnection) As Boolean
        Try
            If cn.State <> ConnectionState.Open Then
                cn.Open()
            End If
            Dim mainConnection As New Microsoft.SqlServer.Management.Common.ServerConnection(cn)
            Dim server As New Server(mainConnection)
            If server.Information.Edition.Trim().ToLowerInvariant().Contains(ExpressEditionName.ToLowerInvariant()) Then
                Dbg.Debug("{0}", server.Information.Edition)
                Return False
            Else
                Dim name As String = server.JobServer.Name
                Dbg.Debug("job server name:{0}", name)
            End If
        Catch ex As UnsupportedFeatureException
            Dbg.Debug("{0}", ex)
            Return False
        Catch e As Exception
            Dbg.Debug("{0}", e)
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
        Return True
    End Function
    Public Function RunFiltrationJob(ByVal jobName As String, rc As ReplicationController) As Integer
        If FromUnitTest Then
            If FromUnitTestSuccess Then Return 0
            Return 1
        End If
        Dim job As IJobProvider = GetJobProvider()
        If (job Is Nothing) Then
            Dbg.Debug("job provider for job {0} is not created", jobName)
            Return 1
        End If
        job.Type = JobType.Filtration
        Dbg.Debug("[{0:f}] starting job {1} ...", CurrentTimeString, jobName)
        Dim result As Integer = job.Run(jobName, rc)
        If result <> 0 Then
            SecurityLog.WriteToEventLogDB(Nothing, SecurityAuditEvent.ProcessStart, False, Nothing, job.LastError, String.Format("Filtration {0} start is failed", jobName), SecurityAuditProcessType.Replication)
            Dbg.Debug("job {0} is not started", jobName)
            Return 1
        End If
        Dbg.Debug("[{0:f}] job {1} is started successfully", CurrentTimeString, jobName)
        SecurityLog.WriteToEventLogDB(Nothing, SecurityAuditEvent.ProcessStart, True, Nothing, Nothing, String.Format("Filtration {0} was started successfully", jobName), SecurityAuditProcessType.Replication)
        SyncLock m_JobQueue
            m_JobQueue.Add(job)
        End SyncLock

    End Function

    Public Function RunReplicationJob(ByVal jobName As String, rc As ReplicationController) As Integer
        If FromUnitTest Then
            If FromUnitTestSuccess Then
                rc.CreateEvent(model.Enums.EventType.ReplicationStarted, Nothing, Nothing)
                Return 0
            End If
            rc.CreateEvent(model.Enums.EventType.ReplicationFailed, Nothing, Nothing)
            Return 1
        End If
        Dim job As IJobProvider = GetJobProvider()
        If (job Is Nothing) Then
            Dbg.Debug("job provider for job {0} is not created", jobName)
            Return 1
        End If

        Dbg.Debug("[{0:f}] starting job {1} ...", CurrentTimeString, jobName)
        job.Type = JobType.Replication
        Dim result As Integer = job.Run(jobName, rc)
        If result <> 0 Then
            'm_JobProvider = Nothing
            rc.CreateEvent(model.Enums.EventType.ReplicationFailed, Nothing, Nothing)
            SecurityLog.WriteToEventLogDB(Nothing, SecurityAuditEvent.ProcessStart, False, Nothing, job.LastError, String.Format("Replication {0} start is failed", jobName), SecurityAuditProcessType.Replication)
            Dbg.Debug("job {0} is not started", jobName)
            Return 1
        End If
        rc.CreateEvent(model.Enums.EventType.ReplicationStarted, Nothing, Nothing)

        Dbg.Debug("[{0:f}] job {1} is started successfully", CurrentTimeString, jobName)
        SecurityLog.WriteToEventLogDB(Nothing, SecurityAuditEvent.ProcessStart, True, Nothing, Nothing, String.Format("Replication {0} was started successfully", jobName), SecurityAuditProcessType.Replication)
        SyncLock m_JobQueue
            m_JobQueue.Add(job)
        End SyncLock
    End Function

    Private Shared Function CurrentTimeString() As String
        Return DateTime.Now.ToString("d/M/yy H:mm:ss.fff")
    End Function
End Class
