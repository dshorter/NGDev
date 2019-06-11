using EIDSS.FlexibleForms.DataBase;
namespace EIDSS.FlexibleForms.Helpers
{
    /// <summary>
    /// ��������� ������ ��� ����������� ���������������� ������
    /// </summary>
    public class ActivityParametersBuffer
    {
        /// <summary>
        /// 
        /// </summary>
        public ActivityParametersBuffer()
        {
            UserDataBuffer = new FlexibleFormsDS.ActivityParametersRow[] { };
        }

        /// <summary>
        /// 
        /// </summary>
        public object IdfsFormTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object IdfsObservation { get; set; }

        /// <summary>
        /// ����� ��� �������� ����� ������ ��� �����������
        /// </summary>
        public FlexibleFormsDS.ActivityParametersRow[] UserDataBuffer { get; set; }

        /// <summary>
        /// ������� ������
        /// </summary>
        public void Clear()
        {
            IdfsFormTemplate = IdfsObservation = null;
            UserDataBuffer = new FlexibleFormsDS.ActivityParametersRow[]{};
        }

        /// <summary>
        /// ���� �� �����
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return ((IdfsFormTemplate == null) || (IdfsObservation == null) || (UserDataBuffer.Length == 0));
        }
    }
}