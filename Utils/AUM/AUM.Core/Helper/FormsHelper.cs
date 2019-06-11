using System.Windows.Forms;
using AUM.Core.Forms;

namespace AUM.Core.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public static class FormsHelper
    {
        /// <summary>
        /// ����� ���� ����� ������ � ������ � ������� ����������
        /// </summary>
        public static DialogResult ShowUpdateServerWindow()
        {
            return (new LoginForm(LoginFormRegime.UpdateServer)).ShowDialog();
        }


        /// <summary>
        /// ����� ���� ����� ������ � ������ � �����-�������������� (Sql Server)
        /// </summary>
        public static DialogResult ShowSqlSettingsWindow()
        {
            return (new LoginForm(LoginFormRegime.MaintSqlSettings)).ShowDialog();
        }

        /// <summary>
        /// ���������� ���� �������� ���������� EIDSS
        /// </summary>
        public static void ShowWaitingForm()
        {
            (new WaitingForm()).ShowDialog(null);
        }
    }
}
