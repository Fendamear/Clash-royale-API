import React, { useState } from 'react';
import ArrowBackIosNewIcon from '@mui/icons-material/ArrowBackIosNew';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import { BarChart } from '@mui/x-charts/BarChart';
import AspectRatio from '@mui/joy/AspectRatio';
import Button from '@mui/joy/Button';
import Card from '@mui/joy/Card';
import CardContent from '@mui/joy/CardContent';
import IconButton from '@mui/joy/IconButton';
import Typography from '@mui/joy/Typography';
import BookmarkAdd from '@mui/icons-material/BookmarkAddOutlined';
import CardOverflow from '@mui/joy/CardOverflow';
import './Homepage.css'

const WeekScroll = () => {
  const defaultData = [{
    "name": "",
    "fame": 0,
    "decksNotUsed": 0,
    "decksUsed": 0
  }]


  const [startDate, setStartDate] = useState(getThursday(new Date()));
  const [toggleSwitch, setToggleSwitch] = useState(false);
  const [decksnotused, setdecksnotused] = useState(defaultData);

  // Function to get the next week's Thursday
  const getNextWeek = () => {
    const nextWeek = new Date(startDate);
    nextWeek.setDate(nextWeek.getDate() + 7); // Add 7 days
    setStartDate(getThursday(nextWeek));
  };

  // Function to get the previous week's Thursday
  const getPrevWeek = () => {
    const prevWeek = new Date(startDate);
    prevWeek.setDate(prevWeek.getDate() - 7); // Subtract 7 days
    setStartDate(getThursday(prevWeek));
  };

  // Function to get the Thursday of the given week
  function getThursday(date) {
    const day = date.getDay();
    const diff = date.getDate() - day + (day === 0 ? -6 : 4); // Adjust to Thursday
    return new Date(date.setDate(diff));
  }

  // Calculate the end date (Monday) based on the start date (Thursday)
  const endDate = new Date(startDate);
  endDate.setDate(endDate.getDate() + 3); // Add 3 days to get to Monday

  const handleToggleSwitch = () => { setToggleSwitch(!toggleSwitch); console.log(toggleSwitch) }

  if (toggleSwitch) {
    return (
      <div>
        <div>
          <div class="wrapperCurrentriverrace">
            <div className='loginform-container'>
              <div class="switch-button">
                <input class="switch-button-checkbox" onClick={handleToggleSwitch} type="checkbox"></input>
                <label class="switch-button-label" for=""><span class="switch-button-label-span">All-Time</span></label>
              </div>
            </div>
          </div>
        </div>
        <div class="wrapperCurrentriverrace">
          <div className='loginform-container'>
            <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
              <ArrowBackIosNewIcon onClick={getPrevWeek} style={{ cursor: 'pointer' }} />
              <p style={{ margin: '0 20px' }}>Week starting on: {startDate.toDateString()}</p>
              <p style={{ margin: '0 20px' }}>Week ending on: {endDate.toDateString()}</p>
              <ArrowForwardIosIcon onClick={getNextWeek} style={{ cursor: 'pointer' }} />
            </div>
            {/* Render your data for the current week using the startDate */}
          </div>
        </div>
        <div class="wrapperCurrentriverrace">
          <div className='loginform-container'>
                <BarChart
                  xAxis={[{
                    scaleType: 'band',
                    categoryGapRatio: 0.6,
                    barGapRatio: 0.1,
                    data: decksnotused.map(item => item.name),
                    tickLabelStyle: {
                      angle: 90,
                      textAnchor: 'start',
                      fontSize: 12,
                      height: 10
                    }
                  }]}
                  series={[{ data: decksnotused.map(item => item.fame) }]}
                  width={1000}
                  height={400}
                />
                <Card
                  size="lg"
                  variant="plain"
                  orientation="horizontal"
                  sx={{
                    textAlign: 'center',
                    maxWidth: '100%',
                    width: 100,
                    // to make the demo resizable
                  }}
                >
                  <CardOverflow
                    variant="solid"
                    color="primary"
                    sx={{
                      flex: '0 0 200px',
                      display: 'flex',
                      flexDirection: 'column',
                      justifyContent: 'center',
                      px: 'var(--Card-padding)',
                    }}
                  >
                    <Typography fontSize="xl4" fontWeight="xl" textColor="#fff">
                      KingKiller
                    </Typography>
                    <Typography fontSize="xl4" fontWeight="xl" textColor="#fff">
                      89
                    </Typography>
                    <Typography textColor="primary.200">
                      Player with highest medals
                    </Typography>
                  </CardOverflow>
                </Card>
                <br></br>
                <Card
                  size="lg"
                  variant="plain"
                  orientation="horizontal"
                  sx={{
                    textAlign: 'center',
                    maxWidth: '100%',
                    width: 100,
                    // to make the demo resizable,,
                  }}
                >
                  <CardOverflow
                    variant="solid"
                    color="primary"
                    sx={{
                      flex: '0 0 200px',
                      display: 'flex',
                      flexDirection: 'column',
                      justifyContent: 'center',
                      px: 'var(--Card-padding)',
                    }}
                  >
                    <Typography fontSize="xl4" fontWeight="xl" textColor="#fff">
                      KingKiller
                    </Typography>
                    <Typography fontSize="xl4" fontWeight="xl" textColor="#fff">
                      89
                    </Typography>
                    <Typography textColor="primary.200">
                      Player with highest medals
                    </Typography>
                  </CardOverflow>
                </Card>
              </div>
            </div>
          </div>
    );
  }

  return (
    <div>
      <div>
        <div class="wrapperCurrentriverrace">
          <div className='loginform-container'>
            <div class="switch-button">
              <input class="switch-button-checkbox" onClick={handleToggleSwitch} type="checkbox"></input>
              <label class="switch-button-label" for=""><span class="switch-button-label-span">All-Time</span></label>
            </div>
          </div>
        </div>
      </div>

    </div>
  );


};

export default WeekScroll;