using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Repository.Cache;
using FuwanViewer.Repository.Fake.Proxy;
using Newtonsoft.Json;
using FuwanViewer.Model.Infrastructure;

namespace FuwanViewer.Repository.Fake
{
    /// <summary>
    /// VisualNovelRepository based on hardcoded data.
    /// </summary>
    /// <remarks>
    /// This repository alternates between two different sets of data
    /// (List A, List B) everytime a GetAll() method is called.
    /// 
    /// Both lists cointain 6 novels, out of which:
    /// 1) Two are the same: Fate Stay night, G-senjou no Maou,
    /// 2) Two differ only by properties: Muv-Luv and Muv-Luv alternative,
    /// 3) Two exists only in one of the lists (Yu-no, sengoku rance, swan song, utawarerumono)
    /// </remarks>
    [DataContract]
    public class FakeFuwaVNRepository : IVisualNovelRepository
    {
        #region Fields and Properties

        private static List<FakeVisualNovel> visualNovelListA;
        private static List<FakeVisualNovel> visualNovelListB;

        [DataMember]
        internal ICache<Uri, BitmapImage> _imageCache;
        [DataMember]
        private int _count = 0;

        public bool CacheImages 
        { 
            get
            {
                return !(_imageCache is NullImageCache);   
            }
            set
            {
                if (value != CacheImages)
                {
                    if (value == true)
                        _imageCache = new SimpleImageCache();
                    else
                    {
                        if (_imageCache is IDisposable)
                            (_imageCache as IDisposable).Dispose();
                        _imageCache = new NullImageCache();
                    }
                }
            }
        }
        public ICache<Uri, BitmapImage> ImageCache { get { return _imageCache; } }

        #endregion // Fields and Properties

        #region Constructors

        static FakeFuwaVNRepository()
        {
            #region List A
            
            visualNovelListA = new List<FakeVisualNovel>(6);

            #region Fate Stay Night

            visualNovelListA.Add(new FakeVisualNovel("Fate Stay Night")
                {
                    Title = "Fate Stay Night",
                    /* Id */
                    Id = 50,
                    /* Description */
                    Description = @"There are seven chosen Masters, and seven classes of Servants. There is only one Holy Grail. If you wish for a miracle. Prove that you are the strongest with your powers.",
                    /* Summary */
                    Summary = @"""The one who obtains the Holy Grail will have any wish come true.\r\n\r\nThe Holy Grail War.\r\nA great ritual that materializes the greatest holy artifact, the Holy Grail.\r\nThere are two conditions to participate in this ritual.\r\nTo be a magus, and to be a \""Master\"" chosen by the Holy Grail.\r\n(The experience as a magus is not questioned if one has the aptitude)\r\n\r\nThere are seven chosen Masters, and seven classes of Servants.\r\n\r\nThere is only one Holy Grail.\r\nIf you wish for a miracle.\r\nProve that you are the strongest with your powers.",
                    /* Company */
                    Company = "Type-Moon",
                    /* Tags */
                    Tags = new List<Tag>() { Tag.Action, Tag.Drama, Tag.Supernatural },
                    /* Sex Content */
                    SexContent = true,
                    /* Featured */
                    IsFeatured = true
                });

            #endregion // Fate Stay Night

            #region G-senjou no Maou

            visualNovelListA.Add(new FakeVisualNovel("G-senjou no Maou")
            {
                Title = "G-senjou no Maou",
                /* Id */
                Id = 35,
                /* Description */
                Description = @"Haru and Kyosuke pursue in a deadly game of cat-and-mouse against world-classed mastermind criminal known only as Maou",
                /* Summary */
                Summary = @"You play the role of Azai Kyousuke, the son of a legendary gangster infamous in the underworld. You spend your time listening to Bach, playing God at school and covertly working for your stepfather, a ruthless financial heavyweight. This idyllic existence is broken when two individuals appear in the city - a beautiful girl named Usami Haru with hair you could get lost in for days, and a powerful international gangster known only as ""Maou"". Almost without delay, the two begin a deadly cat-and-mouse game, bringing you and your friends into the crossfire. Plotting, political intrigue and layer upon layer of interlocking traps are the weapons in this epic battle of wits.",
                /* Company */
                Company = "Akabei Soft2",
                /* Tags */
                Tags = new List<Tag>() { Tag.Comedy, Tag.Drama, Tag.Mystery, Tag.Romance},
                /* Sex Content */
                SexContent = true,
                /* Featured */
                IsFeatured = true
            });

            #endregion // G-senjou no Maou

            #region Muv-Luv

            visualNovelListA.Add(new FakeVisualNovel("Muv-Luv")
            {
                Title = "Muv-Luv",
                /* Id */
                Id = 40,
                /* Description */
                Description = @"Shirogane Takeru's life takes an unexpected turn when he wakes up in bed with a girl (Meiya) one morning. She immediately moves into his house and starts changing his life for the good...",
                /* Summary */
                Summary = @"Shirogane Takeru is a typical high school student with a lazy attitude and a love for the virtual reality mecha battle game Valgern-on. Even though he didn't really wanted it, he is popular in school mainly due to his daily fights with his osananajimi (Sumika) attracting too much attention. His life takes an unexpected turn when he finds a girl (Meiya) he doesn't remember ever meeting in his bed one morning. Whom later revealed to be the heiress of one of the biggest zaibatsu. She immediately moves to his house and starts changing his life for the good with her one-track-mind and unlimited resources...",
                /* Company */
                Company = "Age",
                /* Tags */
                Tags = new List<Tag>() { Tag.Action, Tag.Comedy, Tag.Drama, Tag.SchoolLife},
                /* Sex Content */
                SexContent = true
            });

            #endregion // Muv-Luv

            #region Muv-Luv Alternative

            visualNovelListA.Add(new FakeVisualNovel("Muv-Luv Alternative")
            {
                Title = "Muv-Luv Alternative",
                /* Id */
                Id = 88,
                /* Description */
                Description = @"Muv-Luv Alternative takes place after the events of the original Muv-Luv. Alternative focuses on the life of Shirogane Takeru three years after Muv-Luv.",
                /* Summary */
                Summary = @"Muv-Luv Alternative takes place after the events of the original Muv-Luv. Alternative focuses on the life of Shirogane Takeru three years after Muv-Luv; Takeru wakes up believing that everything that had happened was just a dream. However, he notices something wrong: he has been sent back in time to the beginning of the events in Muv-Luv: Unlimited. But instead of letting a disaster strike again, he decides to help stop it before it even begins. The solution: helping to complete Alternative IV.",
                /* Company */
                Company = "Age",
                /* Tags */
                Tags = new List<Tag>() { Tag.Action, Tag.Drama, Tag.Romance, Tag.ScienceFiction},
                /* Sex Content */
                SexContent = true
            });

            #endregion // Muv-Luv Alternative

            #region Sengoku Rance

            visualNovelListA.Add(new FakeVisualNovel("Sengoku Rance")
            {
                Title = "Sengoku Rance",
                /* Id */
                Id = 45,
                /* Description */
                Description = @"The story takes place during the 4th Sengoku warring states era of Japan, an area of the Rance World which parodies medieval real-world Japan. (Sengoku Rance aka Rance 7)",
                /* Summary */
                Summary = @"The story takes place during the 4th Sengoku (warring states) era of Japan, an area of the Rance World which parodies medieval real-world Japan.\r\n\r\nSengoku Rance picks up where Rance 6 left off. Rance, after saving the nation of Zeth from destruction, ran away so he won't have to marry the princess. He and Sill ended up in Japan for a hotspring trip, upsetting the power balance of Japan, and sets the unification of its provinces in motion. Meanwhile, an ancient evil power stirs from its slumber...\r\n\r\nMeet the alternate reality versions of Oda Nobunaga, Uesugi Kenshin, Takeda Shingen, Tokugawa Ieyasu, Mori Motonari, One Eyed Masamune, Hojo Soun, Sakamoto Ryouma, Yamamoto Isoroku, Saint Francis Xavier, and many more famous Japanese historical figures in the world of Rance!",
                /* Company */
                Company = "Alice Soft",
                /* Tags */
                Tags = new List<Tag>() { Tag.Action, Tag.Comedy, Tag.Gameplay },
                /* Sex Content */
                SexContent = true
            });

            #endregion // Sengoku Rance

            #region Swan Song

            visualNovelListA.Add(new FakeVisualNovel("Swan Song")
            {
                Title = "Swan Song",
                /* Id */
                Id = 195,
                /* Description */
                Description = @"It is a snowing Christmas Eve\u2026 Everything seems so peaceful when a huge earthquake occurs. The earthquake causes the city to be in ruin, and the surviving people need to find ways to stay alive.",
                /* Summary */
                Summary = @"It is a snowing Christmas Eve\u2026 Everything seems so peaceful when a huge earthquake occurs. The earthquake causes the city to be in ruin, and the surviving people need to find ways to stay alive. Some go crazy and rob others, some cling onto God, some gather to live together. The 6 main characters meet at a church while they were trying to find shelter from the snow. What will they see and experience in this extreme situation\u2026?",
                /* Company */
                Company = "FlyingShine",
                /* Tags */
                Tags = new List<Tag>() { Tag.Drama, Tag.Mystery, Tag.Romance},
                /* Sex Content */
                SexContent = true,
                /* Featured */
                IsFeatured = true
            });

            #endregion // Swan Song

            #endregion // List A

            #region List B

            visualNovelListB = new List<FakeVisualNovel>(6);

            #region Fate Stay Night

            visualNovelListB.Add(new FakeVisualNovel("Fate Stay Night")
            {
                Title = "Fate Stay Night",
                /* Id */
                Id = 50,
                /* Description */
                Description = @"There are seven chosen Masters, and seven classes of Servants. There is only one Holy Grail. If you wish for a miracle. Prove that you are the strongest with your powers.",
                /* Summary */
                Summary = @"""The one who obtains the Holy Grail will have any wish come true.\r\n\r\nThe Holy Grail War.\r\nA great ritual that materializes the greatest holy artifact, the Holy Grail.\r\nThere are two conditions to participate in this ritual.\r\nTo be a magus, and to be a \""Master\"" chosen by the Holy Grail.\r\n(The experience as a magus is not questioned if one has the aptitude)\r\n\r\nThere are seven chosen Masters, and seven classes of Servants.\r\n\r\nThere is only one Holy Grail.\r\nIf you wish for a miracle.\r\nProve that you are the strongest with your powers.",
                /* Company */
                Company = "Type-Moon",
                /* Tags */
                Tags = new List<Tag>() { Tag.Action, Tag.Drama, Tag.Supernatural },
                /* Sex Content */
                SexContent = true,
                /* Featured */
                IsFeatured = true
            });

            #endregion // Fate Stay Night

            #region G-senjou no Maou

            visualNovelListB.Add(new FakeVisualNovel("G-senjou no Maou")
            {
                Title = "G-senjou no Maou",
                /* Id */
                Id = 35,
                /* Description */
                Description = @"Haru and Kyosuke pursue in a deadly game of cat-and-mouse against world-classed mastermind criminal known only as Maou",
                /* Summary */
                Summary = @"You play the role of Azai Kyousuke, the son of a legendary gangster infamous in the underworld. You spend your time listening to Bach, playing God at school and covertly working for your stepfather, a ruthless financial heavyweight. This idyllic existence is broken when two individuals appear in the city - a beautiful girl named Usami Haru with hair you could get lost in for days, and a powerful international gangster known only as ""Maou"". Almost without delay, the two begin a deadly cat-and-mouse game, bringing you and your friends into the crossfire. Plotting, political intrigue and layer upon layer of interlocking traps are the weapons in this epic battle of wits.",
                /* Company */
                Company = "Akabei Soft2",
                /* Tags */
                Tags = new List<Tag>() { Tag.Comedy, Tag.Drama, Tag.Mystery, Tag.Romance },
                /* Sex Content */
                SexContent = true,
                /* Featured */
                IsFeatured = true
            });

            #endregion // G-senjou no Maou

            #region Muv-Luv - different properties

            visualNovelListB.Add(new FakeVisualNovel("Muv-Luv B")
            {
                Title = "Muv-Luv",
                /* Id */
                Id = 40,
                /* Description */
                Description = @"==================== THIS IS LIST B ==================\nShirogane Takeru's life takes an unexpected turn when he wakes up in bed with ...",
                /* Summary */
                Summary = @"==================== THIS IS LIST B ================== and a love for the virtual reality mecha battle game Valgern-on. Even though he didn't really wanted it, he is popular in school mainly due to his daily fights with his osananajimi (Sumika) attracting too much attention. His life takes an unexpected turn when he finds a girl (Meiya) he doesn't remember ever meeting in his bed one morning. Whom later revealed to be the heiress of one of the biggest zaibatsu. She immediately moves to his house and starts changing his life for the good with her one-track-mind and unlimited resources...",
                /* Company */
                Company = "Age B",
                /* Tags */
                Tags = new List<Tag>() { Tag.Drama, Tag.SchoolLife },
                /* Sex Content */
                SexContent = true
            });

            #endregion // Muv-Luv - different properties

            #region Muv-Luv Alternative - different properties

            visualNovelListB.Add(new FakeVisualNovel("Muv-Luv Alternative B")
            {
                Title = "Muv-Luv Alternative",
                /* Id */
                Id = 88,
                /* Description */
                Description = @"==================== THIS IS LIST B ================== of the original Muv-Luv. Alternative focuses on the life of Shirogane Takeru three years after Muv-Luv.",
                /* Summary */
                Summary = @"==================== THIS IS LIST B ================== of the original Muv-Luv. Alternative focuses on the life of Shirogane Takeru three years after Muv-Luv; Takeru wakes up believing that everything that had happened was just a dream. However, he notices something wrong: he has been sent back in time to the beginning of the events in Muv-Luv: Unlimited. But instead of letting a disaster strike again, he decides to help stop it before it even begins. The solution: helping to complete Alternative IV.",
                /* Company */
                Company = "Age B",
                /* Tags */
                Tags = new List<Tag>() { Tag.ScienceFiction },
                /* Sex Content */
                SexContent = true
            });

            #endregion // Muv-Luv Alternative - different properties

            #region Utawarerumono

            visualNovelListB.Add(new FakeVisualNovel("Utawarerumono")
            {
                Title = "Utawarerumono",
                /* Id */
                Id = 52,
                /* Description */
                Description = @"Waking up injured, a man finds himself in a small village without memory of who he was. A single event causes him and his fellow villagers to be plunged into a path of war.",
                /* Summary */
                Summary = @"Waking up injured, a man finds himself in a small village after being rescued by an apprentice physician. Having no memory of who he is and a mask he cannot take off, he decides to live with the other villagers peacefully. But peace is fragile, as a single event in their village plunges himself and his fellow villagers into a path of war.",
                /* Company */
                Company = "Leaf",
                /* Tags */
                Tags = new List<Tag>() { Tag.Action, Tag.Drama, Tag.Gameplay, Tag.Romance },
                /* Sex Content */
                SexContent = true,
                /* Featured */
                IsFeatured = true
            });

            #endregion // Utawarerumono

            #region YU-NO

            visualNovelListB.Add(new FakeVisualNovel("YU-NO")
            {
                Title = "YU-NO",
                /* Id */
                Id = 174,
                /* Description */
                Description = @"Takuya Arima is a young student whose father, a historian who has conducted various researches, disappeared recently. During a summer vacation Takuya receives a peculiar package from his missing father.",
                /* Summary */
                Summary = @"Takuya Arima is a young student whose father, a historian who has conducted various researches, disappeared recently. During a summer vacation Takuya receives a peculiar package from his missing father, along with a letter containing information about the existence of various parallel worlds. At first Takuya doesn't take it seriously, but soon he realizes that he possesses a device that allows him to travel to alternate dimensions. Is his father alive, after all? If so, where is he? Full name is: Kono Yo no Hate de Koi wo Utau Shoujo YU-NO",
                /* Company */
                Company = "Elf",
                /* Tags */
                Tags = new List<Tag>() {Tag.Drama, Tag.Mystery, Tag.Romance, Tag.ScienceFiction },
                /* Sex Content */
                SexContent = true
            });

            #endregion // YU-NO

            #endregion // List B
        }

        public FakeFuwaVNRepository() : this(new SimpleImageCache()) { }

        public FakeFuwaVNRepository(ICache<Uri, BitmapImage> imageCache)
        {
            _imageCache = imageCache;
        }

        #endregion // Constructors

        #region Public Interface

        public IEnumerable<VisualNovel> GetAll()
        {
            ChangeList();
            var novels = GetCurrentList();
            var result = new List<FakeVisualNovelProxy>(novels.Count);

            foreach (var novel in novels)
            {
                result.Add(CreateProxyFrom(novel));
            }

            return result;
        }

        public VisualNovel Get(int id)
        {
            FakeVisualNovel novel = GetCurrentList().SingleOrDefault((vn)=> vn.Id == id);

            if (novel == null)
                return null;
            else
                return CreateProxyFrom(novel);
        }

        #endregion // Public Interface

        #region Helper Functions

        private FakeVisualNovelProxy CreateProxyFrom(FakeVisualNovel vn)
        {
            var proxy = new FakeVisualNovelProxy(this, vn.Uris);

            proxy.Id = vn.Id;
            proxy.Title = vn.Title;
            proxy.Company = vn.Company;
            proxy.Summary = vn.Summary;
            proxy.Description = vn.Description;
            proxy.Tags = vn.Tags;
            proxy.SexContent = vn.SexContent;
            proxy.IsFeatured = vn.IsFeatured;

            return proxy;
        }

        private List<FakeVisualNovel> GetCurrentList()
        {
            return (_count % 2) == 1 ? visualNovelListA : visualNovelListB;
        }

        private void ChangeList()
        {
            _count++;
        }

        #endregion // Helper Functions
    }
}
