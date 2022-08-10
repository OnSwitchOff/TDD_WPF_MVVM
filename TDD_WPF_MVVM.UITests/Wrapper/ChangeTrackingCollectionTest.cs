using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_Model;
using TDD_WPF_MVVM.Wrapper;
using Xunit;

namespace TDD_WPF_MVVM.UITests.Wrapper
{
    public class ChangeTrackingCollectionTest
    {
        private List<FriendEmailWrapper> _emails;

        public ChangeTrackingCollectionTest()
        {
            _emails = new List<FriendEmailWrapper>
            {
                new FriendEmailWrapper(new FriendEmail{ Email = "email1"}),
                new FriendEmailWrapper(new FriendEmail{ Email = "email2"}),
            };
        }


        [Fact]
        public void ShouldTrackAddedItems()
        {
            var emailToAdd = new FriendEmailWrapper(new FriendEmail());

            var c = new ChangeTrackingCollection<FriendEmailWrapper>(_emails);
            Assert.Equal(2, c.Count);
            Assert.False(c.IsChanged);

            c.Add(emailToAdd);
            Assert.Equal(3, c.Count);
            Assert.Single(c.AddedItems);
            Assert.Empty(c.RemovedItems);
            Assert.Empty(c.ModifiedItems);
            Assert.Equal(emailToAdd, c.AddedItems.First());

            c.Remove(emailToAdd);
            Assert.Equal(2, c.Count);
            Assert.Empty(c.AddedItems);
            Assert.Empty(c.RemovedItems);
            Assert.Empty(c.ModifiedItems);
            Assert.False(c.IsChanged);
        }

        [Fact]
        public void ShouldTrackRemovedItems()
        {
            var emailToRemove = _emails.First();

            var c = new ChangeTrackingCollection<FriendEmailWrapper>(_emails);

            Assert.Equal(2, c.Count);
            Assert.False(c.IsChanged);

            c.Remove(emailToRemove);
            Assert.Single(c);
            Assert.Empty(c.AddedItems);
            Assert.Single(c.RemovedItems);
            Assert.Empty(c.ModifiedItems);
            Assert.Equal(emailToRemove, c.RemovedItems.First());

            c.Add(emailToRemove);
            Assert.Equal(2, c.Count);
            Assert.Empty(c.AddedItems);
            Assert.Empty(c.RemovedItems);
            Assert.Empty(c.ModifiedItems);
            Assert.False(c.IsChanged);
        }

        [Fact]
        public void ShouldTrackModifiedItems()
        {
            var emailToModify = _emails.First();

            var c = new ChangeTrackingCollection<FriendEmailWrapper>(_emails);

            Assert.Equal(2, c.Count);
            Assert.False(c.IsChanged);

            emailToModify.Email = "email11";
            Assert.Equal(2, c.Count);
            Assert.Empty(c.AddedItems);
            Assert.Empty(c.RemovedItems);
            Assert.Single(c.ModifiedItems);
            Assert.Equal(emailToModify, c.ModifiedItems.First());

            emailToModify.Email = "email1";
            Assert.Equal(2, c.Count);
            Assert.Empty(c.AddedItems);
            Assert.Empty(c.RemovedItems);
            Assert.Empty(c.ModifiedItems);
            Assert.False(c.IsChanged);
        }


        [Fact]
        public void ShouldAcceptChanges()
        {
            var emailToModify = _emails.First();

            var c = new ChangeTrackingCollection<FriendEmailWrapper>(_emails);

            Assert.Equal(2, c.Count);
            Assert.False(c.IsChanged);

            emailToModify.Email = "email11";
            Assert.Equal("email11", emailToModify.Email);
            Assert.Equal("email1", emailToModify.EmailOriginal);
            Assert.True(emailToModify.IsChanged);
            Assert.True(emailToModify.EmailIsChanged);         
            emailToModify.AcceptChanges();
            Assert.Equal("email11", emailToModify.Email);
            Assert.Equal("email11", emailToModify.EmailOriginal);
            Assert.False(emailToModify.IsChanged);
            Assert.False(emailToModify.EmailIsChanged);
        }


        [Fact]
        public void ShouldRejectedChanges()
        {
            var emailToModify = _emails.First();

            var c = new ChangeTrackingCollection<FriendEmailWrapper>(_emails);

            Assert.Equal(2, c.Count);
            Assert.False(c.IsChanged);

            emailToModify.Email = "email11";
            Assert.Equal("email11", emailToModify.Email);
            Assert.Equal("email1", emailToModify.EmailOriginal);
            Assert.True(emailToModify.IsChanged);
            Assert.True(emailToModify.EmailIsChanged);
            emailToModify.RejectChanges();
            Assert.Equal("email1", emailToModify.Email);
            Assert.Equal("email1", emailToModify.EmailOriginal);
            Assert.False(emailToModify.IsChanged);
            Assert.False(emailToModify.EmailIsChanged);
        }

    }
}
