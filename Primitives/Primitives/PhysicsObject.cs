using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Primitives
{
    class PhysicsObject
    {

        #region MVars

        Matrix mRot, mPos, mWorld;

        protected bool gravity = false;
        protected Vector3 vPos, vImpulse, vAccel, vRotat, vVelo, vGrav;
        protected Quaternion qRot;

        public Matrix World { get { return mWorld; } }
        public Vector3 vPosition { get { return vPos; } }
        public Vector3 vVelocity { get { return vVelo; } }
        public Quaternion qRotation { get { return qRot; } }



        #endregion

        #region Make

        public PhysicsObject(Matrix? world, Vector3 gravity)
        {
            mWorld = world ?? Matrix.Identity;
            vGrav = gravity;
            Vector3 temp = Vector3.Zero;
            mWorld.Decompose(out temp, out qRot, out vPos);
        }

        #endregion

        #region Movement

        #region Basic
        //Directly moves object

        public Vector3 PositionAdd(Vector3 posXYZ)
        {
            return vPos += posXYZ;
        }

        public Vector3 PositionSet(Vector3 posXYZ)
        {
            return vPos = posXYZ;
        }

        #endregion

        #region Advanced

        public Vector3 AccelPort(float acceleration)
        {
            return vImpulse += mWorld.Left * acceleration;
        }

        public Vector3 AccelStar(float acceleration)
        {
            return vImpulse += mWorld.Right * acceleration;
        }

        public Vector3 AccelUp(float acceleration)
        {
            return vImpulse += mWorld.Up * acceleration;
        }

        public Vector3 AccelDown(float acceleration)
        {
            return vImpulse += mWorld.Down * acceleration;
        }

        public Vector3 AccelFore(float acceleration)
        {
            return vImpulse += mWorld.Forward * acceleration;
        }

        public Vector3 AccelBack(float acceleration)
        {
            return vImpulse += mWorld.Backward * acceleration;
        }

        public Vector3 Accel(Vector3 acceleration)//
        {
            //-z = forward
            //x = right
            //+y = up

            return vImpulse += acceleration;
        }

        #endregion

        #endregion

        #region Rotation

        #region Basic
        //Directly rotates object

        public Quaternion RotateX(float rotation)
        {
            Quaternion qX = Quaternion.CreateFromAxisAngle(mRot.Right, rotation);
            return qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qX));
        }

        public Quaternion RotateY(float rotation)
        {
            Quaternion qY = Quaternion.CreateFromAxisAngle(mRot.Up, rotation);
            return qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qY));
        }

        public Quaternion RotateZ(float rotation)
        {
            Quaternion qZ = Quaternion.CreateFromAxisAngle(mRot.Forward, rotation);
            return qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qZ));
        }

        public Quaternion RotateArbitrary(float rotation, Vector3 axis)
        {
            Quaternion qr = Quaternion.CreateFromAxisAngle(axis, rotation);
            return qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qr));
        }

        #endregion

        #region Advanced

        public Vector3 PitchUp(float acceleration)
        {
            return vRotat += new Vector3(-acceleration, 0, 0);
        }

        public Vector3 PitchDown(float acceleration)
        {
            return vRotat += new Vector3(acceleration, 0, 0);
        }

        public Vector3 YawStar(float acceleration)
        {
            return vRotat += new Vector3(0, -acceleration, 0);
        }

        public Vector3 YawPort(float acceleration)
        {
            return vRotat += new Vector3(0, acceleration, 0);
        }

        public Vector3 RollPort(float acceleration)
        {
            return vRotat += new Vector3(0, 0, -acceleration);
        }

        public Vector3 RollStar(float acceleration)
        {
            return vRotat += new Vector3(0, 0, acceleration);
        }

        public Vector3 PitchYawRoll(Vector3 pyr)//
        {
            //-z = forward
            //x = right
            //+y = up
            return vRotat += pyr;
        }

        #endregion

        #endregion

        #region U&D

        public void DecelDiv(float velocity, float rotation)
        {
            if (velocity == 0)
                velocity = 1;
            if (rotation == 0)
                rotation = 1;


            vVelo -= vVelo / velocity;
            vRotat -= vRotat / rotation;
        }

        private void UpdateRotation(GameTime gameTime)
        {
            RotateX(vRotat.X);
            RotateY(vRotat.Y);
            RotateZ(vRotat.Z);
        }

        public Matrix Update(GameTime gameTime)
        {
            int time = gameTime.ElapsedGameTime.Milliseconds;

            UpdateRotation(gameTime);

            //Set gravity
            if (gravity)
                vAccel = vGrav;

            //vAccel *= -fAirRes;
            vVelo += vImpulse;

            //Next position (s = vt - 0.5a(t^2))
            vPos += vVelo * time + 0.5f * vAccel * (time * time);
            //Next velocity (v = u + at)
            vVelo = vVelo + vAccel * time;

            //Reset accel
            vImpulse = Vector3.Zero;

            /*
             * m_position = m_position + m_velocity * timeChange + 0.5 * acceleration * (timeChange * timeChange);
             * m_velocity = m_velocity + acceleration * timeChange;
             * m_velocity.y = m_restitution * -m_velocity.y;
             * acceleration = -0.5f * 9.81f * m_velocity.normalisedCopy();
             * 
             * 		
           */

            mRot = Matrix.CreateFromQuaternion(qRot);
            mPos = Matrix.CreateTranslation(vPos);

            return mWorld = Matrix.Identity * mRot * mPos;
        }

        #endregion

    }
}
