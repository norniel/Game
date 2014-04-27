using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Engine
{
    class LoadSaveManager
    {
        internal void LoadSnapshot( uint[,] map )
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

        private void GenerateMap(uint[,] map )
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

                if( map[tmpX,tmpY] != 0 )
                    continue;

                map[tmpX, tmpY] = (uint)((count % 2 == 0) ? 0x0001100 : 0x00000100);

                count--;
            }
        }

    }

}
