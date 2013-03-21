using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;

namespace Software_Developer_Demo
{
    public class Item
    {
        public int itemID;
        public string itemName;
        public Rectangle rect;

        public bool selected = false;

        public virtual void Draw(Graphics gc)
        {}

        public virtual void Set_Point(int x, int y)
        {
            rect.X = x;
            rect.Y = y;
        }

        public Point Get_Position(int index)
        {
            Point result= new Point(0,0);
            int mid_x = (rect.Left + rect.Right) / 2;
            int mid_y = (rect.Top + rect.Bottom) / 2;

            if (index == 0)
            {
                result.X = rect.X;
                result.Y = rect.Y;
            }
            if (index == 1)
            {
                result.X = rect.X;
                result.Y = rect.Bottom;
            }
            if (index == 2)
            {
                result.X = rect.Right;
                result.Y = rect.Top;
            }
            if (index == 3)
            {
                result.X = rect.Right;
                result.Y = rect.Bottom;
            }
            if (index == 4)
            {
                result.X = rect.Left;
                result.Y =  mid_y;
            }
            if (index == 5)
            {
                result.X = rect.Right;
                result.Y = mid_y;
            }
            if (index == 6)
            {
                result.X = mid_x;
                result.Y = rect.Top;
            }
            if (index == 7)
            {
                result.X = mid_x;
                result.Y = rect.Bottom;
            }
            return result;
        }

        public void Resize(int dx, int dy)
        {
            rect.Width += dx;
            rect.Height += dy;

            if (rect.Width < 5)
            {
                rect.Width = 5;
            }
            if (rect.Height < 5)
            {
                rect.Height = 5;
            }
        }

        public virtual int IsRegion(Point p)
        {
            int state = -1;
            if (rect.Left < p.X && rect.Right > p.X && rect.Top < p.Y && rect.Bottom > p.Y)
            {
                state = 0;
            }

            double[] dis = new double[8];

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

            double min_dis = 10000;
            int index_state = 0;
            for (int i = 0; i < 8; i++)
            {
                if (dis[i] < min_dis)
                {
                    min_dis = dis[i];
                    index_state = i + 1;
                }
            }
            if (min_dis < 5)
            {
                return index_state;
            }
            return state;
        }
    }

    public class User : Item
    {
        public User()
        {
            itemID = 0;
            rect.Width = 20;
            rect.Height = 60;
        }

        public override void Draw(Graphics gc)
        {
            Pen mpen = Pens.Black;

            if (selected)
            {
                mpen = Pens.Red;
            }

            base.Draw(gc);
            //head draw
            //rect.Right + rect.Left
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            int headwidth = width  * 4 /6;
            int headheight = height * 2 / 10;
            gc.DrawEllipse(mpen, rect.Left + width / 6, rect.Top, headwidth, headheight);
            
            int handheight = height * 3 / 10;
            //hand draw
            gc.DrawLine(mpen, rect.Left, rect.Top + handheight, rect.Right, rect.Top + handheight);

            //body draw
            gc.DrawLine(mpen, rect.Left + width / 2, rect.Top + headheight, rect.Left + width / 2, rect.Top + headheight + height * 5 / 10);
            // left leg draw
            gc.DrawLine(mpen, rect.Left + width / 2, rect.Top + headheight + height * 5 / 10, rect.Left, rect.Bottom);
            // right leg draw
            gc.DrawLine(mpen, rect.Left + width / 2, rect.Top + headheight + height * 5 / 10, rect.Right, rect.Bottom);
        }
    }

    public class Connection : Item
    {
        public Connection()
        {
            itemID = 2;
        }
        public Item one_item = null;
        public int one_state = -1;

        public Item two_item = null;
        public int two_state = -1;

        public int one_type = 0;
        public int two_type = 0;
        public Point one_p;
        public Point two_p;

        public override int IsRegion(Point p)
        {
            Point p1 = new Point(0, 0);
            if (one_type == 1)
            {
                p1 = one_p;
            }
            if (one_type == 2)
            {
                p1 = one_item.Get_Position(one_state);
            }

            Point p2 = new Point(0, 0);
            if (two_type == 1)
            {
                p2 = two_p;
            }
            if (two_type == 2)
            {
                p2 = two_item.Get_Position(two_state);
            }

            double[] dis = new double[2];
            dis[0] = Math.Sqrt(Math.Pow(p.X - p1.X, 2) + Math.Pow(p.Y - p1.Y, 2));
            dis[1] = Math.Sqrt(Math.Pow(p.X - p2.X, 2) + Math.Pow(p.Y - p2.Y, 2));

            if (dis[0] < 5)
            {
                return 0;
            }
            if (dis[1] < 5)
            {
                return 0;
            }
            double[] abt = new double[2];
            abt[0] = p.X * p1.X + p.Y * p1.Y;
            abt[1] = p.X * p2.X + p.Y * p2.Y;
            double[] bbt = new double[3];
            bbt[0] = p1.X * p1.X + p1.Y * p1.Y;
            bbt[1] = p1.X * p2.X + p1.Y * p2.Y;
            bbt[2] = p2.X * p2.X + p2.Y * p2.Y;

            double alpha = - (2*abt[1] - 2*abt[0] + 2 * bbt[1] - 2 * bbt[2]) / (2*(bbt[0]+bbt[2]-2*bbt[1]));
            if (alpha < 0)
            {
                alpha = 0;
            }
            if (alpha > 1)
            {
                alpha = 1;
            }

            double d1 = alpha * p1.X + (1 - alpha) * p2.X - p.X;
            double d2 = alpha * p1.Y + (1 - alpha) * p2.Y - p.Y;

            double r = Math.Sqrt( d1 * d1 + d2 * d2);

            if (r < 5)
            {
                return 0;
            }
            return -1;
            ///
        }

        public override void Draw(Graphics gc)
        {
            Pen mpen = new Pen(Color.Black, 5); // Pens.Black;

            if (selected)
            {
                mpen.Color = Color.Red;
            }

            base.Draw(gc);
            Point p1 = new Point(0,0);
            if (one_type == 1)
            {
                p1 = one_p;
            }
            if (one_type == 2)
            {
                p1 = one_item.Get_Position(one_state);
            }

            Point p2 = new Point(0, 0);
            if (two_type == 1)
            {
                p2 = two_p;
            }
            if (two_type == 2)
            {
                p2 = two_item.Get_Position(two_state);
            }
            mpen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            gc.DrawLine(mpen,p1,p2);
        }

    }

    public class Case : Item
    {
        public Case()
        {
            itemID = 1;
            rect.Width = 50;
            rect.Height = 20;
        }

        public override void Draw(Graphics gc)
        {
            Pen mpen = Pens.Black;
            if (selected)
            {
                mpen = Pens.Red;
            }
            base.Draw(gc);
            gc.DrawEllipse(mpen, rect);
        }
    }

    public class Item_Factory
    {
        public static Item produce(int itemid)
        {
            if (itemid == 0)
            {
                User muser = new User();
                return muser;
            }
            if (itemid == 1)
            {
                Case mcase = new Case();
                return mcase;
            }

            if (itemid == 2)
            {
                Connection conn = new Connection();
                return conn;
            }
            return null;
        }
    }
}
