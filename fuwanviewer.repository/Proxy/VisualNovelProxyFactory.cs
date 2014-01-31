using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Fuwanovel;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Repository.Cache;

namespace FuwanViewer.Repository.Proxy
{
    /// <summary>
    /// Provides static methods to aid creation of VisualNovelProxy objects.
    /// </summary>
    internal static class VisualNovelProxyFactory
    {
        /// <summary>
        /// Creates VisualNovelProxy based on data in FuwaVisualNovel
        /// and images in FuwaVNRepository.
        /// </summary>
        public static VisualNovelProxy CreateProxy(FuwaVNRepository parentRepository, FuwaVisualNovel fuwaVn)
        {
            var result = new VisualNovelProxy(
                    parentRepository,
                    StringToUri(fuwaVn.images.browse),
                    StringToUri(fuwaVn.images.main),
                    fuwaVn.images.screenshots.ConvertAll((fuwaSS) => StringToUri(fuwaSS.full)));

            result.Id = fuwaVn.id;

            result.Title = fuwaVn.name;
            result.Company = fuwaVn.company;
            result.Tags = fuwaVn.tags.ConvertAll<Tag>(TagConvert);
            result.SexContent = fuwaVn.tl_flags.sex;
            result.Otome = fuwaVn.tl_flags.otome;

            result.IsFeatured = fuwaVn.site_flags.featured;
            result.ReleaseDate = fuwaVn.dates.released.HasValue ? fuwaVn.dates.released.Value : default(DateTime);
            result.TranslateDate = fuwaVn.dates.translated.HasValue ? fuwaVn.dates.translated.Value : default(DateTime);

            result.Description = fuwaVn.texts.description;
            result.Summary = fuwaVn.texts.summary;
            result.TeamListing = fuwaVn.texts.team;
            result.Notes = fuwaVn.texts.notes;
            result.TroubleshootText = fuwaVn.texts.troubleshoot;

            result.FuwanovelUrl = fuwaVn.url;
            result.TranslationHomepageUrl = fuwaVn.homepage;
            result.FaqUrl = fuwaVn.links.faq;
            result.ForumUrl = fuwaVn.links.forum;
            result.WalkthroughUrl = fuwaVn.links.walkthrough;

            result.EgsScore = fuwaVn.egs.score;
            result.EgsVotes = fuwaVn.egs.votes;

            if (fuwaVn.torrent.HasValue)
            {
                var fuwaTorrent = fuwaVn.torrent.Value;
                var modelTorrent = new FuwanViewer.Model.VisualNovels.Torrent();

                modelTorrent.Name = fuwaTorrent.name;
                modelTorrent.Size = fuwaTorrent.size;
                modelTorrent.Url = fuwaTorrent.url;
                modelTorrent.RequiresInstall = fuwaTorrent.requirements.install;
                modelTorrent.RequiresJpLocale = fuwaTorrent.requirements.jp_locale;
                modelTorrent.Patch = fuwaTorrent.requirements.patch;

                result.Torrent = modelTorrent;
            }
            else
            {
                result.Torrent = null;
            }

            return result;
        }

        #region Helper Functions 

        private static Uri StringToUri(string fuwaUri)
        {
            return fuwaUri != null ? new Uri(fuwaUri) : null;
        }

        private static Tag TagConvert(string fuwaTag)
        {
            if (fuwaTag == null)
                return Tag.Unknown;

            // Remove all spaces and '-' from string
            StringBuilder sb = new StringBuilder(fuwaTag.Length);
            for (int i = 0; i < fuwaTag.Length; i++)
            {
                if (Char.IsLetter(fuwaTag[i]))
                    sb.Append(fuwaTag[i]);
            }
            fuwaTag = sb.ToString();

            // Try to parse string, if fail then return Tag.Unknown
            try
            {
                return (Tag)Enum.Parse(typeof(Tag), sb.ToString());
            }
            catch (ArgumentException)
            {
                return Tag.Unknown;
            }
        }

        #endregion // Helper Functions
    }
}
