// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Collections.Generic;
using Markdig.Helpers;
using Markdig.Syntax;

namespace Markdig.Renderers.Roundtrip
{
    /// <summary>
    /// A Normalize renderer for a <see cref="ListBlock"/>.
    /// </summary>
    /// <seealso cref="NormalizeObjectRenderer{ListBlock}" />
    public class ListRenderer : RoundtripObjectRenderer<ListBlock>
    {
        protected override void Write(RoundtripRenderer renderer, ListBlock listBlock)
        {
            renderer.RenderLinesBefore(listBlock);
            if (listBlock.IsOrdered)
            {
                for (var i = 0; i < listBlock.Count; i++)
                {
                    var item = listBlock[i];
                    var listItem = (ListItemBlock) item;
                    renderer.RenderLinesBefore(listItem);

                    var bws = listItem.WhitespaceBefore.ToString();
                    var bullet = listItem.SourceBullet.ToString();
                    var delimiter = listBlock.OrderedDelimiter;
                    renderer.PushIndent(new List<string> { $@"{bws}{bullet}{delimiter}" });
                    renderer.WriteChildren(listItem);
                    renderer.RenderLinesAfter(listItem);
                }
            }
            else
            {
                for (var i = 0; i < listBlock.Count; i++)
                {
                    var item = listBlock[i];
                    var listItem = (ListItemBlock) item;
                    renderer.RenderLinesBefore(listItem);

                    StringSlice bws = listItem.WhitespaceBefore;
                    char bullet = listBlock.BulletType;
                    StringSlice aws = listItem.WhitespaceAfter;

                    renderer.PushIndent(new List<string> { $@"{bws}{bullet}{aws}" });
                    if (listItem.Count == 0)
                    {
                        renderer.Write(""); // trigger writing of indent
                    }
                    else
                    {
                        renderer.WriteChildren(listItem);
                    }
                    renderer.PopIndent();

                    renderer.RenderLinesAfter(listItem);
                }
            }

            renderer.RenderLinesAfter(listBlock);
        }
    }
}