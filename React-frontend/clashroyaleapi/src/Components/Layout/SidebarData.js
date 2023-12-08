import React from 'react'
import * as AiIcons from "react-icons/ai"
import * as GoIcons from "react-icons/go"
import { VscAccount } from "react-icons/vsc"
import { MdLeaderboard} from "react-icons/md"

export const SidebarData = [
    {
        title: "Home",
        icon: <AiIcons.AiFillHome />,
        link: "/",
        cName: 'nav-text'
    },
    {
        title: "Login",
        icon: <AiIcons.AiFillHome />,
        link: "/login",
        cName: 'nav-text'
    },
    {
        title: "Register",
        icon: <AiIcons.AiFillHome />,
        link: "/register",
        cName: 'nav-text'
    },
    {
        title: "Current river race data",
        icon: <VscAccount color='white' />,
        link: "/currentriverrace",
        cName: 'nav-text'
    },
    {
        title: "Mail Preferences",
        icon: <GoIcons.GoCalendar />,
        link: "/MailSubscription",
        cName: 'nav-text'
    },
    {
        title: "Call List",
        icon: <MdLeaderboard />,
        link: "/CallList",
        cName: 'nav-text'
    },
    {
        title: "Clan Members",
        icon: <GoIcons.GoCalendar />,
        link: "/ClanMembers",
        cName: 'nav-text'
    },
    {
        title: "Clan Member Log",
        icon: <GoIcons.GoCalendar />,
        link: "/clanMemberLog",
        cName: 'nav-text'
    },
    {
        title: "River Race Log",
        icon: <GoIcons.GoCalendar />,
        link: "/clanMemberLog",
        cName: 'nav-text'
    },
]