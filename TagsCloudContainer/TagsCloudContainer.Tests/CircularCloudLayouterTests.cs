﻿using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.CloudLayouter;

namespace TagsCloudContainer.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private readonly Point center = new Point(500, 500);
        private CircularCloudLayouter cloudLayouter;

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void Constructing_ShouldThrowOnNegativeCoordinates()
        {
            Action act = () => new CircularCloudLayouter(new Point(0, -1));

            act.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ShouldFailOnRectanglesBiggerThanField()
        {
            var size = new Size(center.X * 2 + 1, center.Y * 2 + 1);

            cloudLayouter.PutNextRectangle(size).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ShouldFailOnRectanglesWithNegativeSize()
        {
            cloudLayouter.PutNextRectangle(new Size(-1, -1)).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ShouldPutFirstRectangleInCenter()
        {
            var rect = cloudLayouter.PutNextRectangle(new Size(10, 10)).Value;

            rect.Contains(center).Should().BeTrue();
        }

        [Test]
        public void PutNextRectangle_RectangleShouldHaveCorrectSize()
        {
            var size = new Size(11, 11);
            var rect = cloudLayouter.PutNextRectangle(size).Value;

            rect.Size.ShouldBeEquivalentTo(size);
        }

        [TestCase(50, 50, 2)]
        [TestCase(50, 50, 10)]
        public void PutNextRectangle_RectanglesShouldNotIntersect(
            int width, int height, int rectanglesCount)
        {
            var sizes = Enumerable.Repeat(new Size(width, height), rectanglesCount);
            var rectangles = sizes.Select(s => cloudLayouter.PutNextRectangle(s).Value).ToList();

            foreach (var rectangle in rectangles)
            {
                rectangles.Count(r => r.IntersectsWith(rectangle)).Should().Be(1);
            }
        }
    }
}