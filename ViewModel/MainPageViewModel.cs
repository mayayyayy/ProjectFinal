﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectApp.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui;
using ProjectApp.Model;
using System.Text.Json;

namespace ProjectApp.ViewModel
{
    public class MainPageViewModel : ViewModel
    {
        private readonly IPopupService popupService;
        readonly Service service;
        private bool first_time = true;

        private List<Post> _posts;
        private Post _selectedPost;

        public List<Post> Posts
        {
            get => _posts;
            set
            {
                _posts = value;
                OnPropertyChanged(nameof(Posts));
            }
        }

        public Post SelectedPost
        {
            get => _selectedPost;
            set
            {
                _selectedPost = value;
                OnPropertyChanged(nameof(SelectedPost));
            }
        }

        public ICommand BtnCommand { get; set; }
        public ICommand PostClickedCommand { get; set; }


        public EventHandler Login { get; set; }

        public MainPageViewModel(IPopupService _popupService, Service _service)
        {
            popupService = _popupService;
            service = _service;

            BtnCommand = new Command(popupService.ShowPopup<LoginViewModel>);

            Login = new(async (s,e) => 
            {
                if (first_time)
                {
                    await popupService.ShowPopupAsync<LoginViewModel>();
                    first_time = false;
                }
                
                Posts = await service.GetAllPosts();
            });

            PostClickedCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("ViewPost", new Dictionary<string, object>()
                {
                    { "Post", SelectedPost}
                });
            });
        }
    }
}
