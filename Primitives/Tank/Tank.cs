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

        float bottomAccel, bottomReverse, bottomRotate, bottomDecel, bottomDerot,
              turretRotate, turretDerot,
              barrelRotate, barrelDerot;
        
        //Camera
        public Vector3 vPosition { get { return vPos; } }
        public Vector3 vVelocity { get { return physBottom.vVelocity; } }
        public Vector3 vCam1P { get { return vPos + Vector3.Transform(new Vector3(0, vBottomS.Y / 2 + vTurretS.Y / 2, (-vTurretS.Z / 2) * 1.5f), primBottom.mRotation * primTurret.mRotation); } }//
        public Vector3 vCam3P { get { return vPos + Vector3.Transform(new Vector3(0, vBottomS.Y / 2 + vTurretS.Y * 4, vTurretS.Z * 4), primBottom.mRotation * primTurret.mRotation); } }//
        

        #endregion
        
        #region Make, Break

        public Tank(Game g, BasicEffect basicEffect, Vector3 pos, Quaternion? preRot, float size, Color primary, Color secondary)
        {
            vPos = pos;
            this.size = size;
            CalculateSizes();

            vTurretT = new Vector3(0, (vBottomS.Y / 2 + vTurretS.Y / 2), 0);
            IvBarrelT = vBarrelT = new Vector3(0, 0, vTurretS.Z / 2 + vBarrelS.Z / 2);

            vPos += vAdjust;
            primBottom = new TankBottom(g, basicEffect, vPos, preRot, vBottomS, primary, secondary, Primitive.Shading.Flat, Primitive.Drawing.WireFace);
            primTurret = new TankTurret(g, basicEffect, vTurretT, null, vTurretS, primary, secondary, Primitive.Shading.Flat, Primitive.Drawing.WireFace);
            primBarrel = new TankBarrel(g, basicEffect, -vBarrelT, null, vBarrelS, primary, secondary, Primitive.Shading.Flat, Primitive.Drawing.WireFace);

            vAdjust = new Vector3(0, vBottomS.Y / 2, 0);
            primBottom.PositionAdd(vAdjust);
            InitSpeeds();
            InitPhysics();
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

            bottomAccel = sU / 256;
            bottomReverse = sU / 512;
            bottomRotate = sU / 384;
            bottomDecel = 32;
            bottomDerot = 4;

            turretRotate = sU / 128;
            turretDerot = 2;

            barrelRotate = sU / 40;
            barrelDerot = 6;
        }

        private void InitPhysics()
        {
            float meter = sU * 16;

            Matrix physBottomM = Matrix.CreateFromQuaternion(primBottom.qRotation) * Matrix.CreateTranslation(primBottom.vPosition);
            physBottom = new PhysicsObject(physBottomM, Vector3.Down * meter * 10);

            Matrix physTurretM = Matrix.CreateFromQuaternion(primTurret.qRotation) * Matrix.CreateTranslation(primTurret.vPosition);
            physTurret = new PhysicsObject(physTurretM, Vector3.Down * meter * 10);

            Matrix physBarrelM = Matrix.CreateFromQuaternion(primBarrel.qRotation) * Matrix.CreateTranslation(primBarrel.vPosition);
            physBarrel = new PhysicsObject(physBarrelM, Vector3.Down * meter * 10);
        }

        #endregion

        #region Movement

        #region Total

        public void MoveFore()
        {
            physBottom.AccelFore(bottomAccel);
        }

        public void MoveBack()
        {
            physBottom.AccelBack(bottomReverse);
        }

        public void PositionAdd(Vector3 posXYZ)
        {
            physBottom.PositionAdd(posXYZ);
        }

        public void PositionSet(Vector3 posXYZ)
        {
            physBottom.PositionSet(posXYZ);
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
            physBottom.YawPort(bottomRotate);
        }

        public void RotateBottomRight()
        {
            physBottom.YawStar(bottomRotate);
        }

        #endregion

        #region Turret

        public void RotateTurretLeft()
        {
            physTurret.YawPort(turretRotate);
        }

        public void RotateTurretRight()
        {
            physTurret.YawStar(turretRotate);
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
            physBarrel.RollStar(barrelRotate);
        }

        public void SpinBarrelCCW()
        {
            physBarrel.RollPort(barrelRotate);
        }

        #endregion

        #endregion

        #region Combat

        public bool Fire()
        {
            return true;
        }


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
            //Physics updates
            physBottom.Update(gameTime);
            physBottom.DecelDiv(bottomDecel, bottomDerot);

            physTurret.Update(gameTime);
            physTurret.DecelDiv(0, turretDerot);

            physBarrel.Update(gameTime);
            physBarrel.DecelDiv(0, barrelDerot);
            
            //Phys to Prim
            vPos = primBottom.vPosition = physBottom.vPosition;
            primBottom.qRotation = physBottom.qRotation;

            primTurret.vPosition = physTurret.vPosition;
            primTurret.qRotation = physTurret.qRotation;

            primBarrel.vPosition = physBarrel.vPosition;
            primBarrel.qRotation = physBarrel.qRotation;

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
