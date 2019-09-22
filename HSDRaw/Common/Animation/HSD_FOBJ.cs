﻿using HSDRaw.Tools;
using System;
using System.Collections.Generic;

namespace HSDRaw.Common.Animation
{
    public enum GXAnimDataFormat
    {
        Float = 0x00,
        Short = 0x20,
        UShort = 0x40,
        SByte = 0x60,
        Byte = 0x80
    }

    public enum JointTrackType
    {
        HSD_A_J_ROTX = 1
, HSD_A_J_ROTY
, HSD_A_J_ROTZ
, HSD_A_J_PATH
, HSD_A_J_TRAX
, HSD_A_J_TRAY
, HSD_A_J_TRAZ
, HSD_A_J_SCAX
, HSD_A_J_SCAY
, HSD_A_J_SCAZ
, HSD_A_J_NODE
, HSD_A_J_BRANCH
, HSD_A_J_SETBYTE0
, HSD_A_J_SETBYTE1
, HSD_A_J_SETBYTE2
, HSD_A_J_SETBYTE3
, HSD_A_J_SETBYTE4
, HSD_A_J_SETBYTE5
, HSD_A_J_SETBYTE6
, HSD_A_J_SETBYTE7
, HSD_A_J_SETBYTE8
, HSD_A_J_SETBYTE9
, HSD_A_J_SETFLOAT0
, HSD_A_J_SETFLOAT1
, HSD_A_J_SETFLOAT2
, HSD_A_J_SETFLOAT3
, HSD_A_J_SETFLOAT4
, HSD_A_J_SETFLOAT5
, HSD_A_J_SETFLOAT6
, HSD_A_J_SETFLOAT7
, HSD_A_J_SETFLOAT8
, HSD_A_J_SETFLOAT9
    }

    public enum GXInterpolationType
    {
        Step            = 0x01,
        Linear          = 0x02,
        HermiteValue    = 0x03,
        Hermite         = 0x04,
        HermiteCurve    = 0x05,
        Constant        = 0x06
    }

    /// <summary>
    /// A key frame descriptor
    /// </summary>
    public class HSD_FOBJ : HSDAccessor
    {
        public override int TrimmedSize => 8;

        public JointTrackType AnimationType { get => (JointTrackType)_s.GetByte(0x00); set => _s.SetByte(0x00, (byte)value); }
        
        private byte ValueFlag { get => _s.GetByte(0x01); set => _s.SetByte(0x01, value); }

        private byte TangentFlag { get => _s.GetByte(0x02); set => _s.SetByte(0x02, value); }

        public byte[] Buffer {
            get => _s.GetReference<HSDAccessor>(0x04)._s.GetData();
            set
            {
                if(value == null)
                {
                    _s.SetReference(0x04, null);
                    return;
                }

                var re = _s.GetReference<HSDAccessor>(0x04);
                if (re == null)
                {
                    re = new HSDAccessor();
                    _s.SetReference(0x04, re);
                }

                re._s.SetData(value);
            }
        }
        
        public uint ValueScale
        {
            get
            {
                return (uint)Math.Pow(2, ValueFlag & 0x1F);
            }
            set
            {
                ValueFlag = (byte)((ValueFlag & 0xE0) | (byte)Math.Log(value, 2));
            }
        }
        
        public uint TanScale
        {
            get
            {
                return (uint)Math.Pow(2, TangentFlag & 0x1F);
            }
            set
            {
                TangentFlag = (byte)((TangentFlag & 0xE0) | (byte)Math.Log(value, 2));
            }
        }
        
        public GXAnimDataFormat ValueFormat
        {
            get
            {
                return (GXAnimDataFormat)(ValueFlag & 0xE0);
            }
            set
            {
                ValueFlag = (byte)((ValueFlag & 0x1F) | (byte)value);
            }
        }
        
        public GXAnimDataFormat TanFormat
        {
            get
            {
                return (GXAnimDataFormat)(TangentFlag & 0xE0);
            }
            set
            {
                TangentFlag = (byte)((TangentFlag & 0x1F) | (byte)value);
            }
        }
        
        public List<FOBJKey> GetDecodedKeys()
        {
            return FOBJFrameDecoder.GetKeys(this);
        }

        public void SetKeys(List<FOBJKey> keys)
        {
            //TODO:
        }

    }
}