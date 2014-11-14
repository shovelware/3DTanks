using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * Make hardpoints changable and add them all in one fell swoop
 * 3P Regular, 1P Barrel, 3P Above, 3P Dramatic
 * 
 * Do something with velocity to tilt tank as you turn
 * Separate velocity direction and acceleration direction
 * Dot product to decide which way to tilt?
 * Not a lot of tilting, but subtle
 * 
 * Work in hovering and jumping
 * Speed boost and general maintennance
 * Unify method names: Position Add/Set etc
 * Some sort of spring for figures
 * 
 */

namespace Primitives
{
    class Tank
    {
        #region MVars

        //Parts
        Primitive primBottom, primTurret, primBarrel;
        public Primitive Bottom { get { return primBottom; } }
        public Primitive Turret { get { return primTurret; } }
        public Primitive Barrel { get { return primBarrel; } }
        PhysicsObject physBottom, physTurret, physBarrel;

        //Construction
        float size, sU;

        Vector3 vPos, vAdjust,
                vBottomS, vTurretS, vBarrelS,
                vTurretT, vBarrelT, IvBarrelT;
        //Movement

        float turretTurn, bottomTurn, barrelRotate;
        
        //Camera
        public Vector3 vPosition { get { return vPos; } set { vPos = value; } }
        public Vector3 vCam1P { get {return vPos + new Vector3(0, vBottomS.Y + vTurretS.Y / 2, 0); } }//
        public Vector3 vCam3P { get { return vPos + Vector3.Transform(new Vector3(0, vBottomS.Y / 2 + vTurretS.Y * 4, vTurretS.Z * 4), primBottom.mRotation * primTurret.mRotation); } }//
        

        #endregion
        
        #region Make, Break

        public Tank(Game g, BasicEffect basicEffect, Vector3 pos)
        {
            vPos = pos;
            size = 128;
            CalculateSizes();

            vTurretT = new Vector3(0, (vBottomS.Y / 2 + vTurretS.Y / 2), 0);
            IvBarrelT = vBarrelT = new Vector3(0, 0, vTurretS.Z / 2 + vBarrelS.Z / 2);

            primBottom = new TankBottom(g, basicEffect, vPos, null, vBottomS, Color.Gray, Color.Cyan, Primitive.Shading.Flat, Primitive.Drawing.WireFace);
            primTurret = new TankTurret(g, basicEffect, vTurretT, null, vTurretS, Color.Gray, Color.Cyan, Primitive.Shading.Flat, Primitive.Drawing.WireFace);
            primBarrel = new TankBarrel(g, basicEffect, -vBarrelT, null, vBarrelS, Color.Gray, Color.Cyan, Primitive.Shading.Flat, Primitive.Drawing.WireFace);

            vAdjust = new Vector3(0, vBottomS.Y / 2, 0);
            InitSpeeds();
        }

        public Tank(Game g, BasicEffect basicEffect, Vector3 pos, Quaternion? preRot, float size, Color primary, Color secondary)
        {
            vPos = pos;
            this.size = size;
            CalculateSizes();

            vTurretT = new Vector3(0, (vBottomS.Y / 2 + vTurretS.Y / 2), 0);
            IvBarrelT = vBarrelT = new Vector3(0, 0, vTurretS.Z / 2 + vBarrelS.Z / 2);

            primBottom = new TankBottom(g, basicEffect, vPos, preRot, vBottomS, primary, secondary, Primitive.Shading.Flat, Primitive.Drawing.WireFace);
            primTurret = new TankTurret(g, basicEffect, vTurretT, null, vTurretS, primary, secondary, Primitive.Shading.Flat, Primitive.Drawing.WireFace);
            primBarrel = new TankBarrel(g, basicEffect, -vBarrelT, null, vBarrelS, primary, secondary, Primitive.Shading.Flat, Primitive.Drawing.WireFace);

            vAdjust = new Vector3(0, vBottomS.Y / 2, 0);
            InitSpeeds();
        }

        private void CalculateSizes()
        {
            sU = size / 32;
            vBottomS = new Vector3(16, 6, 32) * sU;
            vTurretS = new Vector3(16, 4, 16)* sU;
            vBarrelS = new Vector3(3, 3, 16) * sU; 
        }

        private void InitSpeeds()
        {
            turretTurn = 0.05f;
            bottomTurn = 0.025f;
            barrelRotate = 0.2f;
        }

        private void InitPhysics()
        {
            float meter = sU * 16;
            float volume = vBottomS.X * vBottomS.Y * vBottomS.Z;
            float density = 1000f;
            float mass = volume * density;

            physBottom = new PhysicsObject(primBottom.World, new Vector3(0, meter * 10, 0), mass, 0);
        }

        #endregion

        #region Movement

        #region Total

        public void MoveFore()
        {
            PositionAdd(primBottom.World.Forward * 4);//
        }

        public void MoveBack()
        {
            PositionAdd(primBottom.World.Backward * 4);//
        }

        public void PositionAdd(Vector3 add)
        {
            vPos += add;
        }

        public void RotReset()
        {
            primBottom.RotationResetAll();
            primTurret.RotationResetAll();
            primBarrel.RotationResetAll();
        }

        #endregion

        #region Bottom

        public void RotateBottomLeft()
        {
            primBottom.RotateY(bottomTurn);
        }

        public void RotateBottomRight()
        {
            primBottom.RotateY(-bottomTurn);
        }

        #endregion

        #region Turret

        public void RotateTurretLeft()
        {
            primTurret.RotateY(turretTurn);
        }

        public void RotateTurretRight()
        {
            primTurret.RotateY(-turretTurn);
        }

        #endregion

        #region Barrel

        public void RaiseBarrel() //Can probably do this with listupdate but for now get it working
        {
        }

        public void LowerBarrel()
        {
        }

        public void SpinBarrelCW()
        {
            primBarrel.RotateZ(barrelRotate);
        }

        public void SpinBarrelCCW()
        {
            primBarrel.RotateZ(-barrelRotate);
        }

        #endregion

        #endregion

        #region Customise

        protected void SetColFace(Color newCol)
        {
            primBottom.ColFace = newCol;
            primTurret.ColFace = newCol;
            primBarrel.ColFace = newCol;
        }

        protected void SetColWire(Color newCol)
        {
            primBottom.ColWire = newCol;
            primTurret.ColWire = newCol;
            primBarrel.ColWire = newCol;
        }

        public void SetColFaceTar(Color newCol)
        {
            primBottom.ColFaceTar = newCol;
            primTurret.ColFaceTar = newCol;
            primBarrel.ColFaceTar = newCol;
        }

        public void SetColWireTar(Color newCol)
        {
            primBottom.ColWireTar = newCol;
            primTurret.ColWireTar = newCol;
            primBarrel.ColWireTar = newCol;
        }

        #endregion

        #region U&D

        public void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            primBottom.vPosition = vPos + vAdjust;
            primBottom.Update(proj, view, Matrix.Identity, gameTime);
            primTurret.Update(proj, view, primBottom.World, gameTime);
            primBarrel.Update(proj, view, primTurret.World, gameTime);
        }

        public void Draw()
        {
            primBottom.Draw();
            primTurret.Draw();
            primBarrel.Draw();
        }

        #endregion

    }
}
