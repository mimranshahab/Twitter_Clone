document.addEventListener('DOMContentLoaded', fetchTweets);

async function fetchTweets() {
    const response = await fetch('https://localhost:7157/api/tweets');
    const tweets = await response.json();
    const tweetsList = document.getElementById('tweetsList');
    tweetsList.innerHTML = '';
    tweets.forEach(tweet => {
        const tweetElement = document.createElement('div');
        tweetElement.classList.add('tweet');
        tweetElement.innerHTML = `
            <p style="border: 2px solid forestgreen; border-radius: 10px; padding: 15px; background-color: #f0fff0; margin-bottom:5px">
                ${tweet.message}
            </p>

            <small>Posted by User ${tweet.memberId} on ${new Date(tweet.postedDate).toLocaleString()}</small>
        `;
        tweetsList.appendChild(tweetElement);
    });
}
async function postTweet() {
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const message = document.getElementById('message').value;

    if (message.length > 140) {
        alert('Message is too long.');
        return;
    }

    const response = await fetch('https://localhost:7157/api/members', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ email, password })
    });
    const member = await response.json();
    console.log("saved member", member);
    console.log("message", message);

    CreateTweet(member,message);

    //document.getElementById('message').value = '';
    //fetchTweets();
}
async function CreateTweet(memb,msg) {  

    const url = 'https://localhost:7157/api/tweets'; 
    const payload = {
        tweetId: 62699,
        message: msg,
        postedDate: "2024-06-21T07:44:28.720Z",
        memberId: memb.memberId,
        member: {
            memberId: 5,
            email: "string@sad.com",
            password: "string",
            createdAt: "2024-06-21T07:44:28.720Z"
        }
    };

    const requestOptions = {
        method: 'POST', // Or use 'GET', 'PUT', 'DELETE', etc.
        headers: {
            'Content-Type': 'application/json' // Add other headers if necessary
        },
        body: JSON.stringify(payload)
    };

    await fetch(url, requestOptions)
        .then(response => response.json())
        .then(data => console.log(data));

    document.getElementById('message').value = '';
    fetchTweets();
}
async function signIn() {
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value.trim();
    
    if (email.length < 10) {
        alert('Email is too short.');
        return;
    }

    if (password.length < 5) {
        alert('Password is too short.');
        return;
    }

    try {
        const response = await fetch('https://localhost:7157/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email, password })
        });
        
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const rv = await response.json();

        // Save token in local storage
        localStorage.setItem('token', rv.token);
        
        document.getElementById('signInForm').style.display = 'none';
        document.getElementById('tweetForm').style.display = 'block';

        alert('Sign in successful!');
    } catch (error) {
        console.error('Error during sign-in:', error);
        alert('Sign in failed. Please check your email and password.');
    }
}
async function signUp() {
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value.trim();

    if (email.length < 10) {
        alert('Email is too short.');
        return;
    }

    if (password.length < 5) {
        alert('Password is too short.');
        return;
    }

    try {
        const response = await fetch('https://localhost:7157/api/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email, password })
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const rv = await response.json();

        // Save token in local storage
        localStorage.setItem('token', rv.token);

        document.getElementById('signInForm').style.display = 'none';
        document.getElementById('tweetForm').style.display = 'block';

        alert('Sign Up successful!');
    } catch (error) {
        console.error('Error during sign-Up:', error);
        alert('Sign Up failed. Please check your email and password.');
    }
}
