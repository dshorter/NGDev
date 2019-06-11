using System;
using System.Drawing;
using System.Windows.Forms;
using EIDSS.FlexibleForms.DataBase;
using EIDSS.FlexibleForms.DesignerHosting;
using EIDSS.FlexibleForms.Helpers;

namespace EIDSS.FlexibleForms.Components
{
    public class Line : ParameterHost
    {
        /// <summary>
        /// 
        /// </summary>
        public Line()
        {
            InitializeComponent();
            Move += OnLineMoveAndResize;
            SizeChanged += OnLineMoveAndResize;
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            MaximumSize = new Size(0, 0); //new Size(4000, 4000);
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// ������ ������ �� ���������
        /// </summary>
        public static Size DefaultLineSize
        {
            get
            {
                return new Size(1, 1);
            }
        }

        /// <summary>
        /// ������������ ������ ����� ��� ������ ��������
        /// </summary>
        public static Size MaxLineSize
        {
            get
            {
                return new Size(4000, 4000);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLineMoveAndResize(object sender, EventArgs e)
        {
            if (NowLoading) return;
            ParameterHost parameterHost = (sender as ParameterHost) ?? GetParentParameterHost((Control)sender);
            if (parameterHost == null) return;
            FlexibleFormsDS.LinesRow linesRow = (FlexibleFormsDS.LinesRow)parameterHost.Tag;
            if (linesRow == null) return;
            linesRow.intLeft = parameterHost.LeftReal;
            linesRow.intTop = parameterHost.TopReal;
            linesRow.intWidth = parameterHost.Width;
            linesRow.intHeight = parameterHost.Height;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Line
            // 
            this.Appearance.BackColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseBackColor = true;
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MinimumSize = new System.Drawing.Size(1, 1);
            this.Name = "Line";
            this.Size = new System.Drawing.Size(150, 22);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Delete()
        {
            Delete(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deleteLinkedData"></param>
        public override void Delete(bool deleteLinkedData)
        {
            //������� ��� �������
            Parent.Controls.Remove(this);
            //������� ������ � �� �� �������
            if (deleteLinkedData)
            {
                FlexibleFormsDS.LinesRow linesRow = (FlexibleFormsDS.LinesRow) Tag;
                if (HelperFunctions.IsRowAlive(linesRow)) DataHelper.DeleteLineTemplate(MainDbService, linesRow.idfsFormTemplate, linesRow.idfDecorElement, String.Empty);
            }
        }

        /// <summary>
        /// ������ ������� ����� � ������������ � ��������� ��������
        /// </summary>
        /// <param name="parentControl"></param>
        public void ResizeLine(Control parentControl)
        {
            FlexibleFormsDS.LinesRow linesRow = (FlexibleFormsDS.LinesRow)Tag;
            if (linesRow.IsblnOrientationNull()) return;
            if (linesRow.blnOrientation)
            {
                //����� �� ������ ��������� ������������� ��������
                linesRow.intLeft = Left = 0;
                linesRow.intWidth = Width = parentControl.Width; //����� ����� ��������� �������
                linesRow.intHeight = Height = 10;
            }
            if (!linesRow.blnOrientation)
            {
                //����� �� ������ ��������� ������������� ��������
                linesRow.intTop = Top = 0;
                linesRow.intHeight = Height = parentControl.Height; //����� ����� ��������� �������
                linesRow.intWidth = Width = 10;
            }
        }

        /// <summary>
        /// ������ ������� ����� � ������������ � ��������� ��������
        /// </summary>
        public void ResizeLine()
        {
            ResizeLine(Parent);
        }
    }
}
