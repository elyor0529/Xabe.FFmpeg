﻿using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFMpeg.Enums;
using Xunit;

namespace Xabe.FFMpeg.Test
{
    public class ConversionHelperTests

    {
        [Fact]
        public async Task AddAudio()
        {
            string output = Path.ChangeExtension(Path.GetTempFileName(), Extensions.Mp4);

            bool result = await ConversionHelper.AddAudio(Resources.Mp4.FullName, Resources.Mp3.FullName, output)
                                                .Start();

            Assert.True(result);
            var outputInfo = new VideoInfo(output);
            Assert.Equal("aac", outputInfo.AudioFormat);
            Assert.Equal(TimeSpan.FromSeconds(13), outputInfo.Duration);
        }

        [Fact]
        public async Task ExtractAudio()
        {
            string output = Path.ChangeExtension(Path.GetTempFileName(), Extensions.Mp3);
            bool result = await ConversionHelper.ExtractAudio(Resources.Mp4WithAudio.FullName, output)
                                                .Start();

            Assert.True(result);
            var outputInfo = new VideoInfo(output);
            Assert.Equal("mp3", outputInfo.AudioFormat);
            Assert.Null(outputInfo.VideoFormat);
        }

        [Fact]
        public async Task ExtractVideo()
        {
            FileInfo fileInfo = Resources.Mp4WithAudio;
            string output = Path.ChangeExtension(Path.GetTempFileName(), fileInfo.Extension);

            bool result = await ConversionHelper.ExtractVideo(fileInfo.FullName, output)
                                                .Start();

            Assert.True(result);
            var outputInfo = new VideoInfo(output);
            Assert.Equal("h264", outputInfo.VideoFormat);
            Assert.Null(outputInfo.AudioFormat);
        }

        [Fact]
        public async Task JoinWith()
        {
            string output = Path.ChangeExtension(Path.GetTempFileName(), Extensions.Mp4);

            bool result = await ConversionHelper.JoinWith(output, Resources.MkvWithAudio.FullName, Resources.Mp4WithAudio.FullName);

            Assert.True(result);
            var outputInfo = new VideoInfo(output);
            Assert.Equal(TimeSpan.FromSeconds(23), outputInfo.Duration);
            Assert.Equal("h264", outputInfo.VideoFormat);
            Assert.Equal("aac", outputInfo.AudioFormat);
        }

        [Fact]
        public async Task SnapshotTest()
        {
            string output = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Extensions.Png);
            bool result = await ConversionHelper.Snapshot(Resources.Mp4WithAudio.FullName, output)
                                                .Start();

            Assert.True(result);
            Assert.True(File.Exists(output));
            Image snapshot = Image.FromFile(output);
            Assert.Equal(snapshot.Width, snapshot.Width);
        }

        [Fact]
        public async Task ToMp4Test()
        {
            string output = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Extensions.Mp4);

            bool result = await ConversionHelper.ToMp4(Resources.MkvWithAudio.FullName, output)
                                                .Start();

            Assert.True(result);
            var outputInfo = new VideoInfo(output);
            Assert.Equal(TimeSpan.FromSeconds(9), outputInfo.Duration);
            Assert.Equal("h264", outputInfo.VideoFormat);
            Assert.Equal("aac", outputInfo.AudioFormat);
        }

        [Fact]
        public async Task ToOgvTest()
        {
            string output = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Extensions.Ogv);

            bool result = await ConversionHelper.ToOgv(Resources.MkvWithAudio.FullName, output)
                                                .Start();

            Assert.True(result);
            var outputInfo = new VideoInfo(output);
            Assert.Equal(TimeSpan.FromSeconds(9), outputInfo.Duration);
            Assert.Equal("theora", outputInfo.VideoFormat);
            Assert.Equal("vorbis", outputInfo.AudioFormat);
        }

        [Fact]
        public async Task ToTsTest()
        {
            string output = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Extensions.Ts);

            bool result = await ConversionHelper.ToTs(Resources.Mp4WithAudio.FullName, output)
                                                .Start();

            Assert.True(result);
            var outputInfo = new VideoInfo(output);
            Assert.Equal(TimeSpan.FromSeconds(13), outputInfo.Duration);
            Assert.Equal("h264", outputInfo.VideoFormat);
            Assert.Equal("aac", outputInfo.AudioFormat);
        }

        [Fact]
        public async Task ToWebMTest()
        {
            string output = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Extensions.WebM);

            bool result = await ConversionHelper.ToWebM(Resources.Mp4WithAudio.FullName, output)
                                                .Start();

            Assert.True(result);
            var outputInfo = new VideoInfo(output);
            Assert.Equal(TimeSpan.FromSeconds(13), outputInfo.Duration);
            Assert.Equal("vp8", outputInfo.VideoFormat);
            Assert.Equal("vorbis", outputInfo.AudioFormat);
        }
    }
}
