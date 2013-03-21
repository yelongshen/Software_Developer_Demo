using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Software_Developer_Demo
{
    public class Diagram
    {
        public string diagram_name = "";
        public List<Item> objects = new List<Item>();

        public List<Item> selected = new List<Item>();

        public void UnSelected()
        {
            for (int i = 0; i < selected.Count; i++)
            {
                selected[i].selected = false;
            }
            selected.Clear();
        }

        public int Mouse_Move(Point p)
        {
            //1. the mouse is in the selected object.
            for (int i = 0; i < selected.Count; i++)
            {
                int state = selected[i].IsRegion(p);
                if (state != -1)
                {
                    return state;
                }
            }
            return -1;
        }

        public Point Mouse_Move_Create(Point p, ref int mindex, ref int mstate )
        {
            for (int i = 0; i < objects.Count; i++)
            {
                int state = objects[i].IsRegion(p);
                if (state > 0)
                {
                    mindex = i;
                    mstate = state-1;
                    return objects[i].Get_Position(state - 1);
                }
            }
            return p;
        }

        public int Mouse_Select(Point p)
        {
            UnSelected();
            for (int i = 0; i < objects.Count; i++)
            {
                int state = objects[i].IsRegion(p);
                if (state == 0)
                {
                    selected.Add(objects[i]);
                    objects[i].selected = true;
                    return 1;
                    break;
                }
            }
            return 0;
        }

        public void Object_Move(int dx, int dy)
        {
            for (int i = 0; i < selected.Count; i++)
            {
                selected[i].rect.X += dx;
                selected[i].rect.Y += dy;
            }
        }

        public void Object_Resize(int dx, int dy, int index)
        {
            for (int i = 0; i < selected.Count; i++)
            {
                
                if (index == 1)
                {
                    selected[i].rect.X += dx;
                    selected[i].rect.Y += dy;
                    selected[i].Resize(-dx, -dy);
                }
                if (index == 2)
                {
                    selected[i].rect.X += dx;
                    selected[i].Resize(-dx, dy);
                }
                if (index == 3)
                {
                    selected[i].rect.Y += dy;
                    selected[i].Resize(dx, -dy);
                }
                if (index == 4)
                {
                    selected[i].Resize(dx, dy);
                }
                if (index == 5)
                {
                    selected[i].rect.X += dx;
                    selected[i].Resize(-dx, 0);
                }
                if (index == 6)
                {
                    selected[i].Resize(dx, 0);
                }

                if (index == 7)
                {
                    selected[i].rect.Y += dy;
                    selected[i].Resize(0, -dy);
                }

                if (index == 8)
                {
                    selected[i].Resize(0, dy);
                }
            }
        }

        public void Set_AnotherP(Point p)
        {
            int mindex = -1;
            int mstate = -1;
            Point mp = Mouse_Move_Create(p,ref mindex, ref mstate);

            Item mitem = selected[0];
            if (mindex != -1)
            {
                ((Connection)(mitem)).two_type = 2;
                ((Connection)(mitem)).two_state = mstate;
                ((Connection)(mitem)).two_item = objects[mindex];
            }
            else
            {
                ((Connection)(mitem)).two_type = 1;
                ((Connection)(mitem)).two_p = p;
            }
        }

        public void Add_Connect(int itemId, Point p)
        {
            Item mitem = Item_Factory.produce(itemId);
            int mindex = -1;
            int mstate = -1;
            Point mp = Mouse_Move_Create(p,ref mindex, ref mstate);

            if (mindex != -1)
            {
                ((Connection)(mitem)).one_type = 2;
                ((Connection)(mitem)).one_state = mstate;
                ((Connection)(mitem)).one_item = objects[mindex];
            }
            else
            {
                ((Connection)(mitem)).one_type = 1;
                ((Connection)(mitem)).one_p = p;
            }

            objects.Add(mitem);
            UnSelected();
            mitem.selected = true;
            selected.Add(mitem);
        }

        public void add_item(int itemId, Point p)
        {
            Item mitem = Item_Factory.produce(itemId);
            mitem.Set_Point(p.X, p.Y); 
            
            //mitem.rect.X = p.X;
            //mitem.rect.Y = p.Y;
            
            objects.Add(mitem);

            UnSelected();
            mitem.selected = true;
            selected.Add(mitem);
        }

    }
}
