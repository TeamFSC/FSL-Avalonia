using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using MsBox.Avalonia.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System;

using System.Windows;
using Microsoft.VisualBasic;
using StarLight_Core.Authentication;
using System.Threading.Tasks;

namespace FSLAvalonia.Pages;

public partial class Accounts : UserControl
{
    dynamic result;
    dynamic userInfo;

    public Accounts()
    {
        InitializeComponent();
    }

    public class userInfos
    { 
        public static string current { get; set; }
    }


    private async void NewAcc_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var box = MessageBoxManager
          .GetMessageBoxCustom(
            new MsBox.Avalonia.Dto.MessageBoxCustomParams
            {
                ButtonDefinitions = new List<ButtonDefinition>
                {
                    new ButtonDefinition { Name = "离线验证"},
                    new ButtonDefinition { Name = "微软验证"},
                    new ButtonDefinition { Name = "第三方验证（Authlib Injector）"}
                },
                ContentTitle = "选择验证方式",
                ContentMessage = "选择启动游戏时的验证方式，\n请按照需求进行选择！\n如果条件允许，请务必支持正版。\n",
                FontFamily = "Microsoft YaHei UI",

                HyperLinkParams = new MsBox.Avalonia.Dto.HyperLinkParams
                {
                    Text = "购买Minecraft",
                    Action = new Action (() =>
                    {
                        Process.Start( new ProcessStartInfo()
                        {
                            UseShellExecute = true,
                            FileName = "https://minecraft.net"
                        });
                    }
                    )
                }
            }
         );

        result = await box.ShowAsync();

        
        switch (result)
        {
            case "离线验证":

                confirm.IsVisible = true;
                back.IsVisible = true;
                tip1.Text = "玩家名";
                tip1.IsVisible = true;
                text1.Text = string.Empty;
                text1.IsVisible = true;

                text2.IsVisible = false;
                tip2.IsVisible = false;

                break;

            case "微软验证":

                confirm.IsVisible = true;
                back.IsVisible = true;
                tip1.IsVisible = true;
                tip1.Text = "请稍候，验证加载中...";
                text1.Text = string.Empty;
                text1.IsVisible = false;

                text2.IsVisible = false;
                tip2.IsVisible = false;

                var msAuth = new MicrosoftAuthentication("e1e383f9-59d9-4aa2-bf5e-73fe83b15ba0");
                var deviceCodeInfo = await msAuth.RetrieveDeviceCodeInfo();
                Process.Start(new ProcessStartInfo
                {
                    FileName = deviceCodeInfo.VerificationUri,
                    UseShellExecute = true
                });
                
                tip1.Text = "验证地址：" + deviceCodeInfo.VerificationUri + "，\n请输入下方的代码来完成验证，切勿泄露给他人。\n" + deviceCodeInfo.UserCode + "\n验证成功后，请关闭浏览器，等待验证成功！";

                var tokenInfo = await msAuth.GetTokenResponse(deviceCodeInfo);
                

                try
                {
                    userInfo = await msAuth.MicrosoftAuthAsync(tokenInfo, x =>
                    {
                        Console.WriteLine(x);
                    });

                    
                    tip1.Text = "你现在已经成功登录到你的Microsoft账户，可以继续。\n" + "你的账户名为：" + userInfo.Name;
                    await Task.Delay(500);
                    tip1.Text = "Microsoft 验证：" + userInfo.Name + "（" + userInfo.Uuid + "）";
                }
                catch (Exception ex)
                {
                    tip1.Text = "Microsoft 登录失败，如果你还没有购买 Minecraft Java 版，请去 Minecraft 官网或 Microsoft Xbox 购买！\n或者，是因为作者没有测试，导致验证器炸了...\n" + ex.Message;
                }
                break;
        }
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(result == "离线验证" && text1.Text != string.Empty)
        {
            accounts.Items.Add($"离线验证：{text1.Text}");
        }

        if(result == "微软验证")
        {
            try
            {
                accounts.Items.Add($"微软验证：{userInfo.Name}");
            }
            catch { }
        }

        text1.IsVisible = false;
        tip1.IsVisible = false;
        text2.IsVisible = false;
        tip2.IsVisible = false;

        confirm.IsVisible = false;
        back.IsVisible = false;
    }

    private void DelAcc_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(accounts.SelectedItem != null)
        {
            accounts.Items.Remove(accounts.SelectedItem);
        }
        
    }

    private void Button_Clicks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        text1.IsVisible = false;
        tip1.IsVisible = false;
        text2.IsVisible = false;
        tip2.IsVisible = false;

        confirm.IsVisible = false;
        back.IsVisible = false;

    }

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        // 本来想写，后来没内容了awa
    }
}