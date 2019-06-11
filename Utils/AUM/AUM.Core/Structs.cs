namespace AUM.Core
{
  public struct CopyDirData
  {
    public string SourceDir { get; set; }
    public string DestinationDir { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class RunScriptData : IFile
  {
    #region IFile Members

    public string FileName { get; set; }
    public bool Checked { get; set; }

    #endregion

    public override string ToString()
    {
      return FileName;
    }
  }

  public interface IFile
  {
    string FileName { get; set; }
    bool Checked { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class ExecuteData : IFile
  {
    /// <summary>
    /// ����� �� ����� ��������� ���������� ����� ���, ��� ���������� � ���������� �����
    /// </summary>
    public bool NeedWait { get; set; }

    #region IFile Members

    public string FileName { get; set; }
    public bool Checked { get; set; }

    #endregion

    public override string ToString()
    {
      return FileName;
    }
  }

  public struct ChangeConfigData
  {
    public string FileName { get; set; }
    public string Path { get; set; }

    /// <summary>
    /// �������� ��������-�����
    /// </summary>
    public string AttributeKey { get; set; }

    /// <summary>
    /// �������� ��������-��������
    /// </summary>
    public string AttributeValue { get; set; }

    /// <summary>
    /// �������� �����, ������� ��������� � ��������-�����
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// ��������, ������� ��������� � ��������-��������
    /// </summary>
    public string Value { get; set; }

    public bool ValueIsRegularExpression { get; set; }

    /// <summary>
    /// ���� true, �� ���� ��� �������� ��������
    /// </summary>
    public bool DeleteThisNode { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public enum OverwriteFiles
  {
    No,
    Older,
    All,
  }

  /// <summary>
  /// ��������� ������� ���������
  /// </summary>
  public static class Args
  {
    public static string Sync
    {
      get { return "/d"; }
    }

    public static string Admin
    {
      get { return "/a"; }
    }
  }
}