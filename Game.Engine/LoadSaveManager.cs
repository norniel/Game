using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Engine.Objects;

namespace Game.Engine
{
    class LoadSaveManager
    {
        internal void LoadSnapshot( GameObject[,] map )
        {
            GenerateMap( map );
        }

        internal void SaveSnapshot()
        { }

        internal void SaveChanges()
        { }

        internal void LoadHero()
        { }

        internal void SaveHero()
        {}

        private void GenerateMap(GameObject[,] map )
        {
            int width = map.GetLength(0) - 1;
            int height = map.GetLength(1) - 3;

            int count = (int)(width*height*0.35);

            int tmpX, tmpY;
            Random rand = new Random();

            while( count > 0 )
            {
                tmpX = rand.Next(width);
                tmpY = rand.Next(height);

                if( map[tmpX,tmpY] != null )
                    continue;

                map[tmpX, tmpY] = ((count % 3 == 0) ? (FixedObject)new Plant() : (FixedObject)new Tree());

                switch (count % 3)
                {
                    case    0:
                        map[tmpX, tmpY] = (FixedObject)new Tree();
                        break;
                    case 1:
                        map[tmpX, tmpY] = (FixedObject)new Plant();
                        break;
                    case 2:
                        map[tmpX, tmpY] = (FixedObject)new Rock();
                        break;
                }

                count--;
            }

            while (true)
            {
                tmpX = rand.Next(width);
                tmpY = rand.Next(height);

                if (map[tmpX, tmpY] != null)
                    continue;

                map[tmpX, tmpY] = (FixedObject)new Fire();
                break;
            }
        }

    }

}
