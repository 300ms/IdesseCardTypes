module.exports = {
    content: ['../Pages/**/*.cshtml', '../Areas/**/*.cshtml'],
    theme: {
        extend: {
            colors: {
                'mainTitleBlue': '#0071bc',
                'middleTitleBlue': '#407ec0'
            },
        },
    },
    plugins: [
        require('@tailwindcss/forms'),
        require("daisyui")
    ]
}