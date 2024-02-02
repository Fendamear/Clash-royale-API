import logo from './logo.svg';
import './App.css';
import MailSubScription from './Components/MailSubscription/mailsubscription';
import {  Route, Routes, BrowserRouter as Router } from 'react-router-dom';
import Sidebar from './Components/Layout/Sidebar';
import HomePage from './Components/Layout/Homepage';
import RegisterWithClanTag from './Components/Register/RegisterWithClanTag';
import CallList from './Components/CallList/CallList'
import CollapsibleTable from './Components/currentRiverRace/currentriverrace';
import ClanMembers from './Components/ClanMembers/clanMembers';
import ClanMemberLog from './Components/ClanMemberLog/ClanMemberLog';
import Login from './Components/Login/Login'
import RiverRaceLog from './Components/RiverRaceLog/RiverRaceLog'
import BasicBars from './Components/Graphs/Graphs';

function App() {
  return (
    <Router>
    <Sidebar />
      <Routes>
      <Route path='/' element={<HomePage />} />
      <Route path='/MailSubscription' element={<MailSubScription />} />
      <Route path='/Register' element={<RegisterWithClanTag />} />
      <Route path='/Calllist' element={<CallList />} />
      <Route path='/CurrentRiverRace' element={<CollapsibleTable />} />
      <Route path='/ClanMembers' element={<ClanMembers />} />
      <Route path='/ClanMemberLog' element={<ClanMemberLog />} />
      <Route path='/Login' element={<Login />} />
      <Route path='/riverracelog' element={<RiverRaceLog/>} />
      <Route path='/graphs' element={<BasicBars/>} />
      {/* <Route path='/calendar' element={<Race />} />
      <Route path='/Account/:id' element={<Account />} />
      <Route path='/home/predictions' element={<Account />} />
      <Route path='/Login' element={<Login />} />
      <Route path='/register' element={<Register />} />
      <Route path='/prediction/season' element={<LeagueList />} />
      <Route path='/prediction/season/find' element={<FindLeague />} />
      <Route path='/prediction/season/:id/drivers' element={<PredictionLeague />} />
      <Route path='/prediction/season/:id' element={<League />} />
      <Route path='/prediction/season/league/chat/:id' element={<ChatRoom />} />
      <Route path='*' element={<NotFound />} /> */}
    </Routes>
   </Router>
  );
}

export default App;
