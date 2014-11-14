using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{


    class Bullet : Projectile
    {
        public Bullet(Game g, BasicEffect b, Matrix world, float size, Color face, Color wire) : base(g, b, world, size, face, wire) { }

        protected override void InitialiseProjectile()
        {
            typeName = "Bullet";
            primitives = new Primitive[1];

            vSize = new Vector3(8, 8, 16) * sU;

            rootPrim = new BulletPrim(game, effect, vOrigin, rX90 * rX90 * rX90, new Vector3(vSize.X, vSize.Z, vSize.Y), colFace, colWire, shadeMode, drawMode);
        }

        public override void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            //primitives[0].Update(proj, view, Matrix.Identity, gameTime);
            projPhys.AccelFore(2);

            projPhys.Update(gameTime);
            
            primitives[0].World = projPhys.World;
        }

    }
}
