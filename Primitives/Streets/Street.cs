using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Primitives
{
    class StreetFactory
    {
        public static String streetSymbols = "═║╔ ╗╚ ╝╠ ╣╦ ╩ ╬ ";
        public static String straightSymbols = "║═";
        public static String cornerSymbols = "╝╗╔╚";
        public static String teeSymbols = "╣╦╠╩";
        public static String crossSymbols = "╬";
        public static String blankSymbols = " ";

        static Texture2D straightTexture, cornerTexture, teeTexture, crossTexture, blankTexture;
        
        static Rectangle blockSize;

        public static Street makeStreet(char c,Vector2 position)
        {
            Street s = new Street(theGame);
            int streetType = streetSymbols.IndexOf(c);


            if (straightSymbols.Contains(c))
            {
                int i = straightSymbols.IndexOf(c);
                s.Texture = straightTexture;
                s.Rotation = (float)(Math.PI / 2.0 * i);

            }

            if (teeSymbols.Contains(c))
            {
                int i = teeSymbols.IndexOf(c);
                s.Texture = teeTexture;
                s.Rotation = (float)(Math.PI / 2.0) * i;

            }
            if (cornerSymbols.Contains(c))
            {
                int i = cornerSymbols.IndexOf(c);
                s.Texture = cornerTexture;
                s.Rotation = (float)(Math.PI / 2.0) * i;

            }
            if (crossSymbols.Contains(c))
            {
               
                s.Texture = crossTexture;
                s.Rotation = 0;
            }

            if (blankSymbols.Contains(c))
            {

                s.Texture = blankTexture;
                s.Rotation = 0;
            }
            
            s.Position = new Vector3(position.X * blockSize.Width,0,position.Y*blockSize.Height);
            s.Size = blockSize;
       
            s.Init();
            return s;
        }

        static Game theGame;

        public static void Init(Game game, Rectangle bS)
        {
            theGame = game;
            LoadStreetTextures();
            blockSize=bS;
          
        }

        static void LoadStreetTextures()
        {

            ContentManager contentManger = (ContentManager)theGame.Services.GetService(typeof(ContentManager));

            straightTexture = contentManger.Load<Texture2D>("straightroad");
            teeTexture = contentManger.Load<Texture2D>("teeroad");
            cornerTexture = contentManger.Load<Texture2D>("cornerroad");
            crossTexture = contentManger.Load<Texture2D>("crossroad");
            blankTexture = contentManger.Load<Texture2D>("blankroad");


        }

    }

    class Street: GameObject
    {

        Texture2D texture;

        float rotation;
        Matrix rotationMatrix;

        public float Rotation { get { return rotation; } set { rotation = value; } }
        public Texture2D Texture { get { return texture; } set { texture = value; } }


        Rectangle size;

        public Rectangle Size { get { return size; } set { size = value; } }

        public Street(Game game):base(game)
        {
        }

        VertexPositionNormalTexture[] vertices;
        short[] indices;
        int numTriangles;
        int numVertices;


        public override void Init()
        {
            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;

            numVertices = 4;
            vertices = new VertexPositionNormalTexture[numVertices];

            vertices[0].Position = new Vector3(Position.X,0,Position.Z);
            vertices[0].TextureCoordinate = new Vector2(0,0);

            vertices[1].Position = new Vector3(Position.X+size.Width, 0, Position.Z);
            vertices[1].TextureCoordinate = new Vector2(1,0);

            vertices[2].Position = new Vector3(Position.X+size.Width, 0, Position.Z+size.Height);
            vertices[2].TextureCoordinate = new Vector2(1,1);

            vertices[3].Position = new Vector3(Position.X, 0, Position.Z + size.Height);
            vertices[3].TextureCoordinate = new Vector2(0,1);


            numTriangles = 2;
            indices = new short[numTriangles + 2 ];

            int i=0;
            indices[i++] = 0;
            indices[i++] = 1;
            indices[i++] = 3;
            indices[i++] = 2;

            Vector3 centreDisplace = position + new Vector3(size.Width / 2.0f, 0, size.Height / 2.0f);

            rotationMatrix = Matrix.CreateTranslation(-centreDisplace) * Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(centreDisplace);
        }

        public override void Update(GameTime gametime)
        {
        }

        public override void Draw(GameTime gametime, Camera3D camera)
        {
            effect.View = camera.View;

            effect.Projection = camera.Projection;
            effect.World = rotationMatrix;
            effect.Texture = texture;


            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, vertices, 0, numVertices, indices, 0, numTriangles);

            }

        }

        public override void Draw(GameTime gametime, Camera kenmera)
        {
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            effect.View = kenmera.View;

            effect.Projection = kenmera.Projection;
            effect.World = rotationMatrix;
            effect.Texture = texture;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, vertices, 0, numVertices, indices, 0, numTriangles);

            }

        }


    }
}
