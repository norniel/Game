namespace Game.Engine
{
    using System;
    using Objects;
    class LoadSaveManager
    {
        internal void LoadSnapshot( Map map )
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

        private void GenerateMap(Map map)
        {
            var mapSize = map.GetSize();
            int width = (int)mapSize.Width - 1;
            int height = (int)mapSize.Height - 3;

            int count = (int)(width*height*0.35);

            int tmpX, tmpY;
            Random rand = new Random();

            while( count > 0 )
            {
                tmpX = rand.Next(width);
                tmpY = rand.Next(height);

                if( map.GetObjectFromCell(new Point(tmpX, tmpY)) != null )
                    continue;

                switch (count % 4)
                {
                    case    0:
                        map.SetObjectFromCell(new Point(tmpX, tmpY), (FixedObject)new Tree());
                        break;
                    case 1:
                        map.SetObjectFromCell(new Point(tmpX, tmpY), (FixedObject)new Plant());
                        break;
                    case 2:
                        map.SetObjectFromCell(new Point(tmpX, tmpY), (FixedObject)new Rock());
                        break;
                    case 3:
                        map.SetObjectFromCell(new Point(tmpX, tmpY), (FixedObject)new Bush());
                        break;
                }

                count--;
            }

            while (true)
            {
                tmpX = rand.Next(width);
                tmpY = rand.Next(height);

                if (map.GetObjectFromCell(new Point(tmpX, tmpY)) != null)
                    continue;

                map.SetObjectFromCell(new Point(tmpX, tmpY), (FixedObject)new Fire());
                break;
            }
        }

    }

}
