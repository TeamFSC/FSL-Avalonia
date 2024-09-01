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
                    new ButtonDefinition { Name = "������֤"},
                    new ButtonDefinition { Name = "΢����֤"},
                    new ButtonDefinition { Name = "��������֤��Authlib Injector��"}
                },
                ContentTitle = "ѡ����֤��ʽ",
                ContentMessage = "ѡ��������Ϸʱ����֤��ʽ��\n�밴���������ѡ��\n����������������֧�����档\n",
                FontFamily = "Microsoft YaHei UI",

                HyperLinkParams = new MsBox.Avalonia.Dto.HyperLinkParams
                {
                    Text = "����Minecraft",
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
            case "������֤":

                confirm.IsVisible = true;
                back.IsVisible = true;
                tip1.Text = "�����";
                tip1.IsVisible = true;
                text1.Text = string.Empty;
                text1.IsVisible = true;

                text2.IsVisible = false;
                tip2.IsVisible = false;

                break;

            case "΢����֤":

                confirm.IsVisible = true;
                back.IsVisible = true;
                tip1.IsVisible = true;
                tip1.Text = "���Ժ���֤������...";
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
                
                tip1.Text = "��֤��ַ��" + deviceCodeInfo.VerificationUri + "��\n�������·��Ĵ����������֤������й¶�����ˡ�\n" + deviceCodeInfo.UserCode + "\n��֤�ɹ�����ر���������ȴ���֤�ɹ���";

                var tokenInfo = await msAuth.GetTokenResponse(deviceCodeInfo);
                

                try
                {
                    userInfo = await msAuth.MicrosoftAuthAsync(tokenInfo, x =>
                    {
                        Console.WriteLine(x);
                    });

                    
                    tip1.Text = "�������Ѿ��ɹ���¼�����Microsoft�˻������Լ�����\n" + "����˻���Ϊ��" + userInfo.Name;
                    await Task.Delay(500);
                    tip1.Text = "Microsoft ��֤��" + userInfo.Name + "��" + userInfo.Uuid + "��";
                }
                catch (Exception ex)
                {
                    tip1.Text = "Microsoft ��¼ʧ�ܣ�����㻹û�й��� Minecraft Java �棬��ȥ Minecraft ������ Microsoft Xbox ����\n���ߣ�����Ϊ����û�в��ԣ�������֤��ը��...\n" + ex.Message;
                }
                break;
        }
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(result == "������֤" && text1.Text != string.Empty)
        {
            accounts.Items.Add($"������֤��{text1.Text}");
        }

        if(result == "΢����֤")
        {
            try
            {
                accounts.Items.Add($"΢����֤��{userInfo.Name}");
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
        // ������д������û������awa
    }
}