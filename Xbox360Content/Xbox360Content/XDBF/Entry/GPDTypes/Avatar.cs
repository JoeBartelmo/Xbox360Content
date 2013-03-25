using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360Content.XDBF.GPD
{
    namespace Enums
    {
        public enum AssetFlag : uint
        {
            ShowUnachieved = 0x8,
            Unlocked = 0x20000,
            UnlockedOnline = 0x10000
        }
        public enum AssetSubcategory
        {
            CarryableCarryable = 0x44c,
            CarryableFirst = 0x44c,
            CarryableLast = 0x44c,
            CostumeCasualSuit = 0x68,
            CostumeCostume = 0x69,
            CostumeFirst = 100,
            CostumeFormalSuit = 0x67,
            CostumeLast = 0x6a,
            CostumeLongDress = 0x65,
            CostumeShortDress = 100,
            EarringsDanglers = 0x387,
            EarringsFirst = 900,
            EarringsLargehoops = 0x38b,
            EarringsLast = 0x38b,
            EarringsSingleDangler = 0x386,
            EarringsSingleLargeHoop = 0x38a,
            EarringsSingleSmallHoop = 0x388,
            EarringsSingleStud = 900,
            EarringsSmallHoops = 0x389,
            EarringsStuds = 0x385,
            GlassesCostume = 0x2be,
            GlassesFirst = 700,
            GlassesGlasses = 700,
            GlassesLast = 0x2be,
            GlassesSunglasses = 0x2bd,
            GlovesFingerless = 600,
            GlovesFirst = 600,
            GlovesFullFingered = 0x259,
            GlovesLast = 0x259,
            HatBaseballCap = 0x1f6,
            HatBeanie = 500,
            HatBearskin = 0x1fc,
            HatBrimmed = 0x1f8,
            HatCostume = 0x1fb,
            HatFez = 0x1f9,
            HatFirst = 500,
            HatFlatCap = 0x1f5,
            HatHeadwrap = 0x1fa,
            HatHelmet = 0x1fd,
            HatLast = 0x1fd,
            HatPeakCap = 0x1f7,
            Max = 0x270f,
            Min = 0,
            None = 0,
            RingFirst = 0x3e8,
            RingLast = 0x3ea,
            RingLeft = 0x3e9,
            RingRight = 0x3e8,
            ShirtCoat = 210,
            ShirtFirst = 200,
            ShirtHoodie = 0xd0,
            ShirtJacket = 0xd1,
            ShirtLast = 210,
            ShirtLongSleeveShirt = 0xce,
            ShirtLongSleeveTee = 0xcc,
            ShirtPolo = 0xcb,
            ShirtShortSleeveShirt = 0xcd,
            ShirtSportsTee = 200,
            ShirtSweater = 0xcf,
            ShirtTee = 0xc9,
            ShirtVest = 0xca,
            ShoesCostume = 0x197,
            ShoesFirst = 400,
            ShoesFormal = 0x193,
            ShoesHeels = 0x191,
            ShoesHighBoots = 0x196,
            ShoesLast = 0x197,
            ShoesPumps = 0x192,
            ShoesSandals = 400,
            ShoesShortBoots = 0x195,
            ShoesTrainers = 0x194,
            TrousersCargo = 0x131,
            TrousersFirst = 300,
            TrousersHotpants = 300,
            TrousersJeans = 0x132,
            TrousersKilt = 0x134,
            TrousersLast = 0x135,
            TrousersLeggings = 0x12f,
            TrousersLongShorts = 0x12e,
            TrousersLongSkirt = 0x135,
            TrousersShorts = 0x12d,
            TrousersShortSkirt = 0x133,
            TrousersTrousers = 0x130,
            WristwearBands = 0x322,
            WristwearBracelet = 800,
            WristwearFirst = 800,
            WristwearLast = 0x323,
            WristwearSweatbands = 0x323,
            WristwearWatch = 0x321
        }
        public enum AvatarComponentMasks : short
        {
            All = 0x1fff,
            Body = 2,
            Carryable = 0x1000,
            Earrings = 0x400,
            Glasses = 0x100,
            Gloves = 0x80,
            Hair = 4,
            Hat = 0x40,
            Head = 1,
            None = 0,
            Ring = 0x800,
            Shirt = 8,
            Shoes = 0x20,
            Trousers = 0x10,
            Wristwear = 0x200
        }
        public enum AvatarComponentType
        {
            Head,
            Body,
            Hair,
            Shirt,
            Trousers,
            Shoes,
            Hat,
            Gloves,
            Glasses,
            Wristwear,
            Earrings,
            Ring,
            Carryable,
            Count
        }
        public enum AvatarEyebrowTextureIndex
        {
            Angry = 2,
            Confused = 3,
            Invalid = -1,
            MaxIndex = 4,
            Neutral = 0,
            Raised = 4,
            Sad = 1
        }
        public enum AvatarJoint
        {
            AnkleLeft = 11,
            AnkleRight = 15,
            BackLower = 1,
            BackLowerScale = 10,
            BackUpper = 5,
            BackUpperScale = 0x12,
            Base = 0,
            BaseScale = 4,
            CollarLeft = 12,
            CollarRight = 0x10,
            ElbowLeft = 0x19,
            ElbowLeftScale = 0x20,
            ElbowLeftSkin = 0x1f,
            ElbowRight = 0x1c,
            ElbowRightScale = 0x23,
            ElbowRightSkin = 0x22,
            FingerIndexLeft = 0x25,
            FingerIndexLeft1 = 0x33,
            FingerIndexLeft2 = 0x3d,
            FingerIndexRight = 0x2c,
            FingerIndexRight1 = 0x38,
            FingerIndexRight2 = 0x42,
            FingerMiddleLeft = 0x26,
            FingerMiddleLeft1 = 0x34,
            FingerMiddleLeft2 = 0x3e,
            FingerMiddleRight = 0x2d,
            FingerMiddleRight1 = 0x39,
            FingerMiddleRight2 = 0x43,
            FingerRingLeft = 0x27,
            FingerRingLeft1 = 0x35,
            FingerRingLeft2 = 0x3f,
            FingerRingRight = 0x2e,
            FingerRingRight1 = 0x3a,
            FingerRingRight2 = 0x44,
            FingerSmallLeft = 40,
            FingerSmallLeft1 = 0x36,
            FingerSmallLeft2 = 0x40,
            FingerSmallRight = 0x2f,
            FingerSmallRight1 = 0x3b,
            FingerSmallRight2 = 0x45,
            FingerThumbLeft = 0x2b,
            FingerThumbLeft1 = 0x37,
            FingerThumbLeft2 = 0x41,
            FingerThumbRight = 50,
            FingerThumbRight1 = 60,
            FingerThumbRight2 = 70,
            Ground = -1,
            Head = 0x13,
            HipLeft = 2,
            HipLeftScale = 7,
            HipRight = 3,
            HipRightScale = 9,
            INVALID = 0x63,
            KneeLeft = 6,
            KneeLeftScale = 13,
            KneeRightScale = 0x11,
            Neck = 14,
            NeckScale = 0x18,
            PropertyLeft = 0x29,
            PropertyRight = 0x30,
            RightKnee = 8,
            ShoulderLeft = 20,
            ShoulderLeftScale = 0x1a,
            ShoulderLeftSkinScale = 0x1b,
            ShoulderRight = 0x16,
            ShoulderRightScale = 0x1d,
            ShoulderRightSkinScale = 30,
            SpecialLeft = 0x2a,
            SpecialRight = 0x31,
            ToeLeft = 0x15,
            ToeRight = 0x17,
            WristLeft = 0x21,
            WristRight = 0x24
        }
        public enum AvatarLeftEyeTextureIndex
        {
            Angry = 2,
            Blink = 13,
            Confused = 3,
            Happy = 6,
            Invalid = -1,
            Laughing = 4,
            LookDown = 10,
            LookLeft = 12,
            LookRight = 11,
            LookUp = 9,
            MaxIndex = 13,
            Neutral = 0,
            Sad = 1,
            Shocked = 5,
            Sleeping = 8,
            Yawning = 7
        }
        public enum AvatarMouthTextureIndex
        {
            Angry = 2,
            Confused = 3,
            Happy = 6,
            Invalid = -1,
            Laughing = 4,
            MaxIndex = 13,
            Neutral = 0,
            PhoneticAI = 8,
            PhoneticDTH = 13,
            PhoneticEE = 9,
            PhoneticFV = 10,
            PhoneticL = 12,
            PhoneticO = 7,
            PhoneticW = 11,
            Sad = 1,
            Shocked = 5
        }
        public enum AvatarPose
        {
            BodyFixed,
            Body,
            Head,
            LeftHead,
            RightHead,
            LeftHand,
            RightHand,
            LeftShoe,
            RightShoe,
            Torso,
            Pants,
            PosesCount
        }
        public enum AvatarRightEyeTextureIndex
        {
            Angry = 2,
            Blink = 13,
            Confused = 3,
            Happy = 6,
            Invalid = -1,
            Laughing = 4,
            LookDown = 10,
            LookLeft = 11,
            LookRight = 12,
            LookUp = 9,
            MaxIndex = 13,
            Neutral = 0,
            Sad = 1,
            Shocked = 5,
            Sleeping = 8,
            Yawning = 7
        }
        public enum CoordinateSystem
        {
            LeftHanded,
            RightHanded
        }
        public enum DynamicColorType
        {
            Skin,
            Hair,
            Mouth,
            Iris,
            Eyebrow,
            EyeShadow,
            FacialHair,
            SkinFeatures1,
            SkinFeatures2,
            Count
        }
        public enum DynamicTextureType
        {
            Mouth,
            Eye,
            Eyebrow,
            FacialHair,
            EyeShadow,
            SkinFeatures,
            Count
        }
        public enum PoseCameraStyle
        {
            TrackPosition,
            TrackPositionRotation
        }
        public enum RemovableComponents
        {
            NotRemovable,
            Earrings,
            Glasses,
            Gloves,
            Hat,
            Ring,
            Wristwear,
            FacialHair,
            EyeShadow,
            SkinFeatures,
            Prop
        }
    }
    public struct Avatar
    {
        /// <summary>
        /// Struct size
        /// Asset ID
        /// Title ID
        /// Image ID
        /// Flags
        /// Subcategory
        /// Null?
        /// </summary>
        internal uint[] data;
        internal ulong id;
        internal long unlockTime;
        public Entry Entry;
        bool bigEndian;

        public ulong ID { get { return id; } }
        public DateTime UnlockTime { get { return DateTime.FromFileTime(unlockTime); } set { unlockTime = value.ToFileTime(); } }
        public uint AssetID { get { return data[1]; } }
        public uint TitleID { get { return data[2]; } }
        public uint ImageID { get { return data[3]; } }
        public uint Flags { get { return data[4]; } }
        public Enums.AssetSubcategory SubCategory { get { return (Enums.AssetSubcategory)data[5]; } }
        public uint Unknown { get { return data[6]; } }
        public string Name { get { return sdata[0]; } }
        public string UnlockedDescription { get { return sdata[1]; } }
        public string LockedDescription { get { return sdata[2]; } }
        internal string[] sdata;
        public bool GetFlag(Enums.AssetFlag Flag) { return ((Flags & Flags) > 1); }
        public void SetFlag(Enums.AssetFlag Flag, bool i) { if (i != GetFlag(Flag)) data[4] ^= (uint)Flag; }
        public Avatar(ref IO io, Entry e)
        {
            this.Entry = e;
            bigEndian = io.Endianness == Endian.Big;
            data = new uint[7];
            data[0] = io.ReadUInt32();//struct size
            data[1] = io.ReadUInt32();//asset id
            id = io.ReadUInt64();
            data[2] = io.ReadUInt32();//title id
            data[3] = io.ReadUInt32();//image id
            data[4] = io.ReadUInt32();//flags
            unlockTime = io.ReadInt64();
            data[5] = io.ReadUInt32();//sub cat
            data[6] = io.ReadUInt32();//unk
            sdata = new string[3];
            sdata[0] = io.ReadZString(true);
            sdata[1] = io.ReadZString(true);
            sdata[2] = io.ReadZString(true);
            this.Entry.length = (uint)((byte[])this).LongLength;
        }

        public static explicit operator byte[](Avatar ava)
        {
            List<byte> bytes = new List<byte>(ava.data[0].ToBytes(ava.bigEndian));
            bytes.AddRange(ava.data[1].ToBytes(ava.bigEndian));
            bytes.AddRange(ava.id.ToBytes(ava.bigEndian));
            bytes.AddRange(ava.data[2].ToBytes(ava.bigEndian));
            bytes.AddRange(ava.data[3].ToBytes(ava.bigEndian));
            bytes.AddRange(ava.data[4].ToBytes(ava.bigEndian));
            bytes.AddRange(ava.unlockTime.ToBytes(ava.bigEndian));
            bytes.AddRange(ava.data[5].ToBytes(ava.bigEndian));
            bytes.AddRange(ava.data[6].ToBytes(ava.bigEndian));
            bytes.AddRange(ava.sdata[0].ToBytes(true, ava.bigEndian));
            bytes.AddRange(new byte[2] { 0, 0 });
            bytes.AddRange(ava.sdata[1].ToBytes(true, ava.bigEndian));
            bytes.AddRange(new byte[2] { 0, 0 });
            bytes.AddRange(ava.sdata[2].ToBytes(true, ava.bigEndian));
            bytes.AddRange(new byte[2] { 0, 0 });
            return bytes.ToArray();
        }
    }
}
