using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Software_Developer_Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        UML_Project mproject = new UML_Project();

        public class item_control
        {
            //id 0 user
            //id 1 user case
            //id 2 link.
            public int selected_item = -1;

            public bool IsMouseDown = false;

            public Diagram selected = null;

            public Point p1;
            public Point p2;
            public Point p3;
            public Point p4;

            public int mouse_state = -1;

            public void Mouse_Move(Point p)
            {
                if (selected != null)
                {
                    if (IsMouseDown && selected_item == 2)
                    {
                        selected.Set_AnotherP(p);
                    }
                    /*
                    if (selected_item != -1 && !IsMouseDown)
                    {
                        if (selected_item == 2)
                        {
                            p1 = selected.Mouse_Move_Create(p);
                        }
                    }
                    else*/
                    if (selected_item == -1 && !IsMouseDown)
                    {
                        int state = selected.Mouse_Move(p);
                        mouse_state = state;
                    }
                    else if (IsMouseDown && mouse_state == 0)
                    {
                        p2 = p;
                        int mx = p2.X - p1.X;
                        int my = p2.Y - p1.Y;
                        selected.Object_Move(mx, my);
                        p1 = p;
                    }
                    else if (IsMouseDown && mouse_state >= 1 && mouse_state <= 8)
                    {
                        p2 = p;
                        int mx = p2.X - p1.X;
                        int my = p2.Y - p1.Y;
                        selected.Object_Resize(mx, my, mouse_state);
                        p1 = p;
                    }
                    
                }
            }

            public void Mouse_Down(Point p)
            {
                if (mouse_state == -1 && selected_item != -1)
                {
                    // if it is the line.
                    if (selected_item == 2 && selected != null)
                    {
                        selected.Add_Connect(selected_item, p);
                        IsMouseDown = true;
                        p1 = p;
                    }
                    else if (selected_item != -1 && selected != null)
                    {
                        selected.add_item(selected_item, p);
                        selected_item = -1;
                    }
                }

                if (mouse_state == -1 && selected_item == -1)
                {
                    if (selected != null)
                    {
                        selected.Mouse_Select(p);
                    }
                }
                if (mouse_state == 0)
                {
                    p1 = p;
                    IsMouseDown = true;
                }
                if (mouse_state >= 1 && mouse_state <= 8)
                {
                    p1 = p;
                    IsMouseDown = true;
                }
            }

            public void Mouse_Up(Point p)
            {
                if (IsMouseDown)
                {
                    IsMouseDown = false;
                }
                selected_item = -1;
            }
        }


        //Cursor = Cursors.SizeAll;
        public class Cursor_Control
        {
            /*
            dis[0] = Math.Sqrt(Math.Pow(p.X - rect.Left, 2) + Math.Pow(p.Y - rect.Top, 2));
            dis[1] = Math.Sqrt(Math.Pow(p.X - rect.Left, 2) + Math.Pow(p.Y - rect.Bottom, 2));
            dis[2] = Math.Sqrt(Math.Pow(p.X - rect.Right, 2) + Math.Pow(p.Y - rect.Top, 2));
            dis[3] = Math.Sqrt(Math.Pow(p.X - rect.Right, 2) + Math.Pow(p.Y - rect.Bottom, 2));

            int mid_x = (rect.Left + rect.Right) / 2;
            int mid_y = (rect.Top + rect.Bottom) / 2;

            dis[4] = Math.Sqrt(Math.Pow(p.X - rect.Left, 2) + Math.Pow(p.Y - mid_y, 2));
            dis[5] = Math.Sqrt(Math.Pow(p.X - rect.Right, 2) + Math.Pow(p.Y - mid_y, 2));
            dis[6] = Math.Sqrt(Math.Pow(p.X - mid_x, 2) + Math.Pow(p.Y - rect.Top, 2));
            dis[7] = Math.Sqrt(Math.Pow(p.X - mid_x, 2) + Math.Pow(p.Y - rect.Bottom, 2));
            */
            public static Cursor get_control(int index)
            {
                if (index == -1)
                {
                    return Cursors.Default;
                }
                if (index == 0)
                {
                    return Cursors.SizeAll;
                }
                if (index == 1 || index == 4)
                {
                    return Cursors.SizeNWSE;
                }
                if (index == 2 || index == 3)
                {
                    return Cursors.SizeNESW;
                }
                if (index == 5 || index == 6)
                {
                    return Cursors.SizeWE;
                }
                if (index == 7 || index == 8)
                {
                    return Cursors.SizeNS;
                }
                return Cursors.Default;
            }
        }

        item_control mcontrol = new item_control();

        private void button11_Click(object sender, EventArgs e)
        {
            mcontrol.selected_item = 2;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            mcontrol.selected_item = 1;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            mcontrol.selected_item = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //
            string mname = mproject.addUser_Cases();
            mcontrol.selected = mproject.User_Cases[mproject.User_Cases.Count - 1];
            listBox3.Items.Add(mname);
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //
            mcontrol.Mouse_Down(e.Location);
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            mcontrol.Mouse_Move(e.Location);
            panel1.Cursor = Cursor_Control.get_control(mcontrol.mouse_state);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mcontrol.Mouse_Up(e.Location);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //
            if (mcontrol.selected != null)
            {
                Graphics gc = panel1.CreateGraphics();
                ///viewing all the control.
                gc.Clear(Color.White);
                for (int i = 0; i < mcontrol.selected.objects.Count;i++ )
                {
                    mcontrol.selected.objects[i].Draw(gc);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button5_Click(sender, e);
        }
        
    }
}
